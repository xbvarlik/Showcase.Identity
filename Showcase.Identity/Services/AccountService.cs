using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Context;
using Showcase.Identity.Data.Entities;
using Showcase.Identity.Data.Mappers;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Exceptions;
using Showcase.Identity.InMemoryCache;

namespace Showcase.Identity.Services;

public class AccountService(
    UserService userService,
    UserRoleService userRoleService,
    SessionService sessionService,
    SignInManager<User> signInManager,
    TokenService tokenService,
    EmailService emailService,
    ICacheManager cacheManager,
    UserCredentials credentials,
    IHttpContextAccessor contextAccessor,
    MssqlContext mssqlContext)
{
    public async Task<UserTokenViewModel> LoginAsync(LoginModel model, string agent,
        CancellationToken cancellationToken = default)
    {
        var user = await userService.FindByEmailAsync(model.Email);

        if (user is null)
            throw new ShowcaseUnauthorizedException("User not found");

        var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
            throw new ShowcaseUnauthorizedException("Invalid password");

        var userTokenViewModel = await GetUserTokenViewModel(user);

        await sessionService.CreateSessionAsync(
            new SessionCreateModel(userTokenViewModel.AuthProperties, userTokenViewModel, agent, user),
            cancellationToken);

        return userTokenViewModel;
    }

    public async Task<UserTokenViewModel> RefreshTokenAsync(RefreshTokenCreateModel model,
        CancellationToken cancellationToken = default)
    {
        var user = await userService.FindByEmailAsync(model.Email);

        if (user is null)
            throw new ShowcaseUnauthorizedException("User not found");

        var session = await sessionService.GetAsync(model.Email, model.ClientId, cancellationToken);

        if (session is null)
            throw new ShowcaseUnauthorizedException("Session not found");

        if (session.IsExpired)
        {
            await sessionService.DeleteSessionAsync(model.Email, model.ClientId, cancellationToken);
            throw new ShowcaseUnauthorizedException("Session expired");
        }

        var userTokenViewModel = await GetUserTokenViewModel(user);

        session.AuthProperties = userTokenViewModel.AuthProperties;

        await sessionService.UpdateSessionAsync(session, cancellationToken);

        return userTokenViewModel;
    }

    public async Task LogoutAsync(LogoutModel model, CancellationToken cancellationToken = default)
    {
        await sessionService.DeleteSessionAsync(model.Email, model.ClientId, cancellationToken);
    }

    private async Task<UserTokenViewModel> GetUserTokenViewModel(User user)
    {
        var userRoles = await userService.GetRolesAsync(user);

        var userTokenViewModel = new UserTokenViewModel
        {
            UserId = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Roles = userRoles.ToList()
        };

        var token = tokenService.GenerateToken(user, userRoles);

        userTokenViewModel.AuthProperties = token;

        return userTokenViewModel;
    }

    public async Task<UserTokenViewModel> RegisterUserAsync(RegisterModel model,
        CancellationToken cancellationToken = default)
    {
        // var userWithEmail = await userService.FindByEmailAsync(model.Email);
        //
        // if (userWithEmail is not null) throw new ShowcaseAlreadyExistsException();

        var entity = await userService.CreateAsync(model.ToCreateModel(), cancellationToken)
                     ?? throw new ShowcaseNullException();

        await userRoleService.AssignUserRoleAsync(entity, cancellationToken);

        try
        {
            //await SendVerificationOtpAsync(model.Email, cancellationToken);
            return await LoginAsync(new LoginModel { Email = model.Email, Password = model.Password }, "",
                cancellationToken);
        }
        catch (Exception e)
        {
            throw new ShowcaseUnauthorizedException("User could not be authenticated.", e);
        }
    }

    public async Task SendVerificationOtpAsync(string email, CancellationToken cancellationToken = default)
    {
        var model = new OtpControlModel
        {
            Email = email,
            OtpCode = GenerateOtpCode()
        };
        await emailService.SendEmailAsync("barisvarlik19@gmail.com", email, "Verify your email on Showcase",
            $"Your one time password to verify your account: {model.OtpCode}");

        cacheManager.Set(email, model);
    }

    public async Task<bool> VerifyOtpAsync(OtpControlModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var otpModel = await cacheManager.GetAsync<OtpControlModel>(model.Email, cancellationToken);
            return otpModel?.OtpCode == model.OtpCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static int GenerateOtpCode()
    {
        var random = new Random();
        var code = random.Next(100000, 999999);
        return code;
    }

    public async Task SendResetPasswordOtpAsync(string email, CancellationToken cancellationToken = default)
    {
        var model = new OtpControlModel
        {
            Email = email,
            OtpCode = GenerateOtpCode()
        };
        await emailService.SendEmailAsync("emre@Showcaseuni.com", email, "Reset password on Showcase",
            $"Your one time password to reset your password: {model.OtpCode}");

        cacheManager.Set(email, model);
    }

    public async Task UpdateSignalRConnectionAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        if (contextAccessor is null) throw new ShowcaseOperationalException();

        var userSession = await sessionService.GetByTokenAsync(
                              contextAccessor.HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1],
                              cancellationToken)
                          ?? throw new ShowcaseOperationalException();

        userSession.SignalRConnectionId = connectionId;

        var clientClaim =
            contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(AuthConstants.Claims.ClientId))
            ?? throw new ShowcaseNullException();

        var clientId = Guid.Parse(clientClaim.Value);

        cacheManager.Update(clientId.ToString(), userSession);
        await sessionService.UpdateSessionAsync(userSession, cancellationToken);
    }

    public async Task DeleteAccountAsync(CancellationToken cancellationToken = default)
    {
        if (credentials is null || credentials.UserId == Guid.Empty)
            throw new ShowcaseUnauthorizedException();

        var user = await mssqlContext.Users.Where(x => x.Id == credentials.UserId && !x.IsDeleted)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new ShowcaseNotFoundException(ExceptionConstants.Business.UserNotFoundError,
                       ExceptionConstants.Locations.WebBff);

        try
        {
            user.IsDeleted = true;

            mssqlContext.Users.Update(user);
            await mssqlContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new ShowcaseDatabaseException(e);
        }
    }
}