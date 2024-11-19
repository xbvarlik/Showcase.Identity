using System.Diagnostics.CodeAnalysis;
using System.Text;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Showcase.Identity.Data;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Context;
using Showcase.Identity.Data.Entities;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Exceptions;
using Showcase.Identity.Exceptions.Handlers;
using Showcase.Identity.InMemoryCache;
using Showcase.Identity.Middleware;
using Showcase.Identity.Services;
using Showcase.Identity.Settings;

namespace Showcase.Identity;

public static class Bootstrapper
{
    public static void BootstrapBuilder(this WebApplicationBuilder builder)
    {
        builder.Services.BootstrapServices(builder.Configuration);
        builder.AddHealthChecks();
    }

    private static void BootstrapServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddInMemoryCache(configuration);
        services.AddControllersConfig();
        services.AddDatabases(configuration);
        services.AddApplicationSwagger();
        services.AddServices();
        services.AddEndpointsApiExplorer();
        services.AddApplicationAuthentication(configuration);
        services.AddAuthorization();
        services.AddSingleton<UserCredentials>();
    }

    public static void BootstrapApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x => x.EnableFilter());
        }

        app.ConfigureApplicationPipeline();
        
        app.UseHealthChecks();
    }
    
    private static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("I am alive!"))
            .AddCheck("Version", () => HealthCheckResult.Healthy(Environment.Version.ToString()))
            .AddCheck("Environment", () => HealthCheckResult.Healthy(builder.Environment.EnvironmentName))
            .AddCheck("ready", () => HealthCheckResult.Healthy("I am ready!"));
    }

    private static void UseHealthChecks(this WebApplication app)
    {
        app.UseHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = check => check.Name == "self" || check.Name == "Version" || check.Name == "Environment",
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecks("/ready", new HealthCheckOptions
        {
            Predicate = check => check.Name == "ready" || check.Name == "Version" || check.Name == "Environment",
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    private static void ConfigureApplicationPipeline(this WebApplication app)
    {
        app.ConfigureApplicationCors();
        app.ConfigureApplicationSwagger();
        app.UseExceptionHandler(_ => { });
        app.UseAuthentication();
        app.UseMiddleware<JwtHandlerMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
    }
    
    [SuppressMessage("ReSharper", "InvertIf")]
    private static void ConfigureApplicationSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(o => o.EnableFilter());
        } 
    }
    
    private static void ConfigureApplicationCors(this WebApplication app)
    {
        app.UseCors( 
            policyBuilder => policyBuilder.WithOrigins(BootstrapperConstant.FrontendPath)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
        );
    }
    
    private static void AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, Role>(options => { options.SignIn.RequireConfirmedAccount = false; })
            .AddEntityFrameworkStores<MssqlContext>()
            .AddUserManager<UserService>()
            .AddDefaultTokenProviders();

        var tokenSettings = configuration.GetSection("JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
        
        if(tokenSettings is null)
            throw new ShowcaseOperationalException(ExceptionConstants.System.Operational.JwtBearerTokenSettingsError);
        
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters =  new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings!.PrimarySecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenSettings.Issuer,
                    ValidAudience = tokenSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
    
    private static void AddControllersConfig(this IServiceCollection services)
    {
        services.AddControllers(
            options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            }
        );
    }
    
    private static void AddApplicationSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Description = "Standard Authorization header using the Bearer scheme(\"bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "oauth2",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }
}