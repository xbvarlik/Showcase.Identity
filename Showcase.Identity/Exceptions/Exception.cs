using System.Diagnostics.CodeAnalysis;

namespace Showcase.Identity.Exceptions;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public abstract class ShowcaseException : Exception
{
    protected ShowcaseException(string code, string level, Exception? innerException = null) : base(null, innerException)
    {
        Code = code;
        Level = level;
    }
    
    protected ShowcaseException(string code, string level, string location, Exception? innerException = null) : base(null, innerException)
    {
        Code = code;
        Level = level;
        Location = location;
    }

    public string Code { get; } = null!;

    public string Level { get; } = null!;
    
    public string Location { get; } = null!;
}

[Serializable]
public class ShowcaseSystemException : ShowcaseException
{
    public ShowcaseSystemException(string code, Exception? innerException = null) : base(code, ExceptionConstants.Levels.System, innerException)
    {
    }
    
    public ShowcaseSystemException(string code, string location, Exception? innerException = null) : base(code, ExceptionConstants.Levels.System, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseDatabaseException : ShowcaseSystemException
{
    public ShowcaseDatabaseException(Exception? innerException = null) : base(ExceptionConstants.System.Database.DatabaseError, innerException)
    {
    }
    
    public ShowcaseDatabaseException(string code, string location, Exception? innerException = null) : base(code, location, innerException)
    {
    }
    
    public ShowcaseDatabaseException(string location, Exception? innerException = null) : base(ExceptionConstants.System.Database.DatabaseError, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseOperationalException : ShowcaseSystemException
{
    public ShowcaseOperationalException(Exception? innerException = null) : base(ExceptionConstants.System.Operational.OperationalError, innerException)
    {
    }

    public ShowcaseOperationalException(string errorCode, Exception? innerException = null) : base(errorCode, innerException)
    {
    }
    
    public ShowcaseOperationalException(string errorCode, string location, Exception? innerException = null) : base(errorCode, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseUnauthorizedException : ShowcaseSystemException
{
    public ShowcaseUnauthorizedException(Exception? innerException = null) : base(ExceptionConstants.Business.UnauthorizedError, innerException)
    {
    }
    
    public ShowcaseUnauthorizedException(string code, string location, Exception? innerException = null) : base(code, location, innerException)
    {
    }
    
    public ShowcaseUnauthorizedException(string location, Exception? innerException = null) : base(ExceptionConstants.Business.UnauthorizedError, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseBusinessException : ShowcaseException
{
    public ShowcaseBusinessException(string code, Exception? innerException = null) : base(code, ExceptionConstants.Levels.Business, innerException)
    {
    }
    
    public ShowcaseBusinessException(string code, string location, Exception? innerException = null) : base(code, ExceptionConstants.Levels.Business, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseForbiddenException : ShowcaseBusinessException
{
    public ShowcaseForbiddenException(Exception? innerException = null) : base(ExceptionConstants.Business.ForbiddenError, innerException)
    {
    }
    
    public ShowcaseForbiddenException(string location, Exception? innerException = null) : base(ExceptionConstants.Business.ForbiddenError, location, innerException)
    {
    }
    
    public ShowcaseForbiddenException(string errorCode, string location, Exception? innerException = null) : base(errorCode, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseNotFoundException : ShowcaseBusinessException
{
    public ShowcaseNotFoundException(Exception? innerException = null) : base(ExceptionConstants.Business.NotFoundError, innerException)
    {
    }
    
    public ShowcaseNotFoundException(string location, Exception? innerException = null) : base(ExceptionConstants.Business.NotFoundError, location, innerException)
    {
    }
    
    public ShowcaseNotFoundException(string errorCode, string location, Exception? innerException = null) : base(errorCode, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseNullException : ShowcaseBusinessException
{
    public ShowcaseNullException(Exception? innerException = null) : base(ExceptionConstants.Business.NullError, innerException)
    {
    }
    
    public ShowcaseNullException(string location, Exception? innerException = null) : base(ExceptionConstants.Business.NullError, location, innerException)
    {
    }
    
    public ShowcaseNullException(string errorCode, string location, Exception? innerException = null) : base(errorCode, location, innerException)
    {
    }
}

[Serializable]
public class ShowcaseAlreadyExistsException : ShowcaseBusinessException
{
    public ShowcaseAlreadyExistsException(Exception? innerException = null) : base(ExceptionConstants.Business.AlreadyExistsError, innerException)
    {
    }
    
    public ShowcaseAlreadyExistsException(string location, Exception? innerException = null) : base(ExceptionConstants.Business.AlreadyExistsError, location, innerException)
    {
    }
    
    public ShowcaseAlreadyExistsException(string errorCode, string location, Exception? innerException = null) : base(errorCode, location, innerException)
    {
    }
}
