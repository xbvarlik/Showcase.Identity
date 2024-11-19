namespace Showcase.Identity.Data.Constants;

public static class BootstrapperConstant
{
    public const string AuthorizationPolicyName = "PolicyForApiControllers";
    public const string AuthorizationRequiredClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    //public const string AuthorizationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    public const string MssqlContextName = "MssqlContext";
    public const string MongoClientName = "MongoClient";
    public const string CosmosContextName = "CosmosContext";
    public const string CosmosContextDatabaseName = "CosmosSettings:DatabaseName";
    public const string JwtBearerTokenSettingsName = "JwtBearerTokenSettings";

    public const string SwaggerRouteTemplate = "api/swagger/{documentName}/swagger.json";
    public const string SwaggerRoutePrefix = "api/swagger";

    public const string OriginWhiteList = "OriginWhiteList";

    public const string JwtAudience = "JwtBearerTokenSettings:Audience";
    public const string JwtPrimarySecretKey = "JwtBearerTokenSettings:SecretKey";

    public static readonly string ExceptionPath = "/error";

    public const string SignalRPath = "/chat-hub";
    public const string FrontendPath = "http://localhost:3000";
    public const string DefaultLanguage = "TR";

    public static class Language
    {
        public const string Turkish = "TR";
        public const string English = "EN";
    } 
    
    public static readonly Guid SystemUserId = Guid.NewGuid(); // TODO: Change this value
    public static Guid UserRoleId = Guid.Parse("f3ab6ba5-7a9b-4bb4-b952-d7cb450fde92");
    public static Guid RealEstateAdminRoleId = Guid.Parse("838b8386-1ccf-4af3-9aed-2130a7bdac7e"); 
}

public static class CookieConstant
{
    public const string AccessDeniedPath = "/account/access-denied";
    public const string LoginPath = "/account/login";
    public const string LogoutPath = "/account/logout";
    public const string CookieName = "ApplicationGatewayAffinity";
}