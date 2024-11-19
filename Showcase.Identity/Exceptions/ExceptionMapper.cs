namespace Showcase.Identity.Exceptions;

public static class ExceptionMapper
{
    public static ExceptionModel ToExceptionViewModel(this Exception exception)
    {
        return exception switch
        {
            ShowcaseForbiddenException forbiddenException => new ExceptionModel
            {
                Code = ExceptionConstants.Business.ForbiddenError,
                Message = forbiddenException.Code,
                Detail = forbiddenException.Message,
                Location = forbiddenException.Source ?? ExceptionConstants.Locations.WebBff
            },
            ShowcaseNullException nullException => new ExceptionModel
            {
                Code = ExceptionConstants.Business.NullError,
                Message = nullException.Code,
                Detail = nullException.Message,
                Location = nullException.Source ?? ExceptionConstants.Locations.WebBff
            },
            ShowcaseNotFoundException notFoundException => new ExceptionModel
            {
                Code = ExceptionConstants.Business.NotFoundError,
                Message = notFoundException.Code,
                Detail = notFoundException.Message,
                Location = notFoundException.Source ?? ExceptionConstants.Locations.WebBff
            },
            ShowcaseUnauthorizedException unauthorizedException => new ExceptionModel
            {
                Code = ExceptionConstants.Business.UnauthorizedError,
                Message = unauthorizedException.Code,
                Detail = unauthorizedException.Message,
                Location = unauthorizedException.Source ?? ExceptionConstants.Locations.WebBff
            },
            ShowcaseOperationalException operationalException => new ExceptionModel
            {
                Code = ExceptionConstants.System.Operational.OperationalError,
                Message = operationalException.Code,
                Detail = operationalException.Message,
                Location = operationalException.Source ?? ExceptionConstants.Locations.WebBff
            },
            ShowcaseDatabaseException databaseException => new ExceptionModel
            {
                Code = ExceptionConstants.System.Database.DatabaseError,
                Message = databaseException.Message,
                Detail = databaseException.Message,
                Location = databaseException.Source ?? ExceptionConstants.Locations.WebBff
            },
            ShowcaseBusinessException forbiddenException => new ExceptionModel
            {
                Code = ExceptionConstants.Business.CommonError,
                Message = forbiddenException.Code,
                Detail = forbiddenException.Message,
                Location = forbiddenException.Source ?? ExceptionConstants.Locations.WebBff
            },
            // HttpRequestException httpRequestException => new ExceptionModel
            // {
            //     Code = ExceptionConstants.System.Operational.HttpRequestError,
            //     Detail = httpRequestException.Message.FormatMessage(),
            //     Location = ExceptionConstants.Locations.Showcase
            // },
            _ => new ExceptionModel
            {
                Code = ExceptionConstants.System.InternalServerError,
                Detail = exception.Message,
                Location = exception.Source ?? ExceptionConstants.Locations.WebBff
            }
        };
    }
    
    // [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed", Justification = "LINQ is used to find and replace the service name in the message.")]
    // private static string FormatMessage(this string message)
    // {
    //     var serviceNames = new Dictionary<string, string>
    //     {
    //        
    //         { "localhost:6001", ExceptionConstants.Locations.Showcase },
    //         { "localhost:5001", ExceptionConstants.Locations.Hrm },
    //         { "localhost:5005", ExceptionConstants.Locations.Bank },
    //         { "localhost:5002", ExceptionConstants.Locations.Log },
    //         { "localhost:5004", ExceptionConstants.Locations.Score },
    //         { "localhost:6002", ExceptionConstants.Locations.Notification },
    //     };
    //
    //     serviceNames.FirstOrDefault(x => 
    //     { 
    //         message = message.Contains(x.Key) ? message.Replace(x.Key, x.Value) : message;
    //         return message.Contains(x.Key);
    //     });
    //
    //     return message;
    // }
}