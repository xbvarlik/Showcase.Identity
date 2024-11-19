namespace Showcase.Identity.Services;

public static class Bootstrapper
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<EmailService>();
        services.AddScoped<AccountService>();
        services.AddScoped<SessionService>();
        services.AddScoped<TokenService>();
        services.AddScoped<UserService>();
        services.AddScoped<UserRoleService>();
        services.AddScoped<UserRoleServiceGuard>();
    }
}