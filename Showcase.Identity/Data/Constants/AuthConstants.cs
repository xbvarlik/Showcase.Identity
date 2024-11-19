namespace Showcase.Identity.Data.Constants;

public static class AuthConstants
{
    public const string JwtClaimType = "urn:implika:jwt";
    public const string ClientId = "ClientId";

    public static class Claims
    {
        public const string JwtClaimType = "urn:talentfinance:jwt";
        public const string ClientId = "ClientId";
        public const string RefreshTokenClaimType = "urn:talentfinance:xgaslan";
        public const string EmailAddress = "emailaddress";
    }
    
    public static class Items
    {
        public const string SessionProperties = "SessionProperties";
    }
    
    public static class Headers
    {
        public const string UserAgent = "User-Agent";
        public const string Authorization = "Authorization";
        public const string UserId = "UserId";
        public const string CompanyId = "CompanyId";
        public const string CompanySegmentId = "CompanySegmentId";
        public const string TenantId = "TenantId";
        public const string Roles = "Roles";
        public const string Email = "Email";
        public const string LogonLanguage = "LogonLanguage";
        public const string ClientId = "ClientId";
    }
    
    public static class ExcludedPaths
    {
        public static readonly string[] JwtHandlerExcludedPaths = 
            [
                "/api/account/refresh-token", 
                "/api/account/login", 
                "/alive", 
                "/ready",
                "/api/account/register-user", 
                "/api/localization",
                "/chat-hub",
                "/chat-hub/negotiate"
            ];
    }

    public static class UserRoles
    {
        public const string SuperAdmin = "Super Admin";
        public const string Consumer = "Consumer";
        public const string Seller = "Seller";
        public const string User = "User";

        public static readonly Guid UserRoleId = Guid.Parse("f3ab6ba5-7a9b-4bb4-b952-d7cb450fde92");
    }

    public const string Comma = ", ";
}  