namespace Showcase.Identity.Exceptions;

public static class ExceptionConstants
{
    public static class System
    {
        public const string SystemError = "System.Error";
        public const string InternalServerError = "System.Internal.Server.Error";
        public const string EncryptionError = "System.Encryption.Error";

        public static class Database
        {
            public const string DatabaseError = "System.Database.Error";
        }

        public static class Operational
        {
            public const string OperationalError = "System.Operational.Error";
            public const string CosmosConfigurationError = "System.Cosmos.Configuration.Error" ;
            public const string MssqlConfigurationError = "System.Mssql.Configuration.Error";
            public const string JwtBearerTokenSettingsError = "System.JwtBearerTokenSettings.Error";
            public const string HttpRequestError = "System.HttpRequest.Error";
            public const string FileOperationError = "System.File.Operation.Error";
        }
    }
    
    public static class Business
    {
        public const string CommonError = "Business.Common.Error";
        public const string UserEmailMinLengthError = "Business.User.Email.Min.Length.Error";
        public const string UserEmailMaxLengthError = "Business.User.Email.Max.Length.Error";
        public const string UserPasswordMinLengthError = "Business.User.Password.Min.Length.Error";
        public const string UserPasswordMaxLengthError = "Business.User.Password.Max.Length.Error";
        public const string UserRoleMinLengthError = "Business.User.Role.Min.Length.Error";
        public const string NotFoundError = "Business.Not.Found.Error";
        public const string NullError = "Business.Null.Error";
        public const string ForbiddenError = "Business.Forbidden.Error";
        public const string AlreadyExistsError = "Business.Already.Exists.Error";
        public const string UnauthorizedError = "Business.Unauthorized.Error";
        public const string BusinessInternalServerError = "Business.Internal.Server.Error";
        public const string GroupCannotBeEmptyError = "Business.Group.Cannot.Be.Empty.Error";
        public const string AllowedImageCountExceededError = "Business.Allowed.Image.Count.Exceeded.Error";
        public const string ImageNotFoundError = "Business.Image.Not.Found.Error";
        public const string InvalidChatError = "Business.Invalid.Chat.Error";
        public const string UserNotFoundError = "Business.User.NotFound.Error";
        public const string RoleNotFoundError = "Business.Role.NotFound.Error";
        public const string UserAlreadyExistsError = "Business.User.Already.Exists.Error";
        public const string QuantityMustBePositiveError = "Business.Quantity.Must.Be.Positive.Error";
        public const string ScheduledDateCannotBeInThePastError = "Business.Scheduled.Date.Cannot.Be.In.The.Past.Error";
        public const string ProductNotFoundError = "Business.Product.Not.Found.Error";
        public const string UserSessionError = "Business.User.Session.Not.Found.Error";
        public const string InvalidInputError = "Business.Invalid.Input.Error";
        public const string ValueCannotBeEmptyError = "Business.Value.Cannot.Be.Empty.Error";

    }
    
    public static class Locations
    {
        public const string WebBff = "Showcase.WebBFF";
    }
    
    public static class Levels
    {
        public const string System = "System.Exception";
        public const string Business = "Business.Exception";
    }
    
    public const string CommonErrorMessage = "Something went wrong";
}