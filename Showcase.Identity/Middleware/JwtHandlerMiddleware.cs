using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Documents;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Exceptions;
using Showcase.Identity.Services;
using Showcase.Identity.Settings;

namespace Showcase.Identity.Middleware;

public class JwtHandlerMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly JwtBearerTokenSettings _jwtSettings = configuration.GetSection("JwtBearerTokenSettings").Get<JwtBearerTokenSettings>()!;
    public async Task InvokeAsync(HttpContext context)
    {
        if (AuthConstants.ExcludedPaths.JwtHandlerExcludedPaths.Contains(context.Request.Path.Value) || CheckAnonymous(context))
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers.Authorization.ToString().Split(" ")[1];

        var emailClaim = context.User.Claims.FirstOrDefault(x => x.Type.Contains(AuthConstants.Claims.EmailAddress))
                         ?? throw new ShowcaseNullException();
        var email = emailClaim.Value;

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        var clientClaim = context.User.Claims.FirstOrDefault(x => x.Type.Equals(AuthConstants.Claims.ClientId))
                          ?? throw new ShowcaseNullException();

        var clientId = Guid.Parse(clientClaim.Value);
        var languageId = context.Request.Headers[AuthConstants.Headers.LogonLanguage].ToString();

        try
        {
            _ = ValidateToken(token);
            await ValidateSessionAsync(context, email, clientId, token, languageId);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await next(context);
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.PrimarySecretKey)),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero
        };
        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        return principal;
    }

    private static async Task ValidateSessionAsync(HttpContext context, string email, Guid clientId, string jwt,
        string languageId)
    {
        using var scope = context.RequestServices.CreateScope();
        var sessionService = scope.ServiceProvider.GetRequiredService<SessionService>();
        var userCredentials = scope.ServiceProvider.GetRequiredService<UserCredentials>();

        var session = await sessionService.GetAsync(email, clientId);

        if (session is null)
            throw new ShowcaseUnauthorizedException();

        if (session.IsExpired)
        {
            var accountService = scope.ServiceProvider.GetRequiredService<AccountService>();
            await accountService.LogoutAsync(new LogoutModel(session.Email, clientId));

            throw new ShowcaseUnauthorizedException();
        }

        if (session.AuthProperties.Token != jwt)
            throw new ShowcaseUnauthorizedException();

        context.Items[AuthConstants.Items.SessionProperties] = session;
        MapSession(session, userCredentials, languageId);
        await sessionService.SetAsync(session.ClientId, session);
    }

    private static void MapSession(Session session, UserCredentials userCredentials, string languageId)
    {
        userCredentials.UserId = session.UserId;
        userCredentials.UserRoles = session.UserRoles.ToList();
        userCredentials.CampusId = session.CampusId;
        userCredentials.LanguageId =
            string.IsNullOrEmpty(languageId) ? BootstrapperConstant.DefaultLanguage : languageId;
    }

    private static bool CheckAnonymous(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint is null) return true;

        var actionDescriptor =
            endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();

        if (actionDescriptor is null) throw new ShowcaseOperationalException();

        return actionDescriptor.MethodInfo
                   .GetCustomAttributes(inherit: true)
                   .Any(attr => attr is AllowAnonymousAttribute)
               ||
               actionDescriptor.ControllerTypeInfo
                   .GetCustomAttributes(inherit: true)
                   .Any(attr => attr is AllowAnonymousAttribute);
    }
}