using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Showcase.Identity.Data.Enums;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Exceptions;

namespace Showcase.Identity.Result;

public sealed class ServiceResult : IServiceResult
{
    public bool IsSuccess { get; private init; }

    public Result? Result { get; private init; }

    public ErrorResult? ErrorResult { get; private init; }

    public static IServiceResult Ok()
        => new ServiceResult { IsSuccess = true, Result = Result.Ok() };

    public static IServiceResult Created()
        => new ServiceResult { IsSuccess = true, Result = Result.Created() };

    public static IServiceResult Accepted()
        => new ServiceResult { IsSuccess = true, Result = Result.Accepted() };

    public static IServiceResult NoContent()
        => new ServiceResult { IsSuccess = true, Result = Result.NoContent() };

    public static IServiceResult SystemError(ExceptionModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.SystemError(data) };

    public static IServiceResult GlobalSystemError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.GlobalSystemError() };

    public static IServiceResult Unauthorized()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.UnauthorizedError() };
    
    public static IServiceResult Unauthorized(ExceptionModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.UnauthorizedError(data) };

    public static IServiceResult BusinessError(ExceptionModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.BusinessError(data) };

    public static IServiceResult GlobalBusinessError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.GlobalBusinessError() };

    public static IServiceResult NotFound()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.NotFoundError() };
    
    public static IServiceResult NotFound(ExceptionModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.NotFoundError(data) };

    public static IServiceResult AlreadyExistsError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.AlreadyExistsError() };
    
    public static IServiceResult AlreadyExistsError(ExceptionModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.AlreadyExistsError(data) };
    
    public static IServiceResult ForbiddenError()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.ForbiddenError() };
    
    public static IServiceResult ForbiddenError(ExceptionModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.ForbiddenError(data) };
    public static IServiceResult BadRequest()
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.BadRequest() };
    
    public static IServiceResult BadRequest(ExceptionModel data)
        => new ServiceResult { IsSuccess = false, ErrorResult = ErrorResult.BadRequest(data) };
}

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public sealed class ServiceResult<T> : IServiceResult<T> where T : class
{
    [BindNever, Newtonsoft.Json.JsonIgnore, JsonIgnore]
    Result? IServiceResult.Result { get; }

    public bool IsSuccess { get; private init; }

    public Result<T>? Result { get; private init; }

    public ErrorResult? ErrorResult => null;

    public static IServiceResult<T> Ok(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.Ok(data) };

    public static IServiceResult<T> Created(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.Created(data) };

    public static IServiceResult<T> Accepted(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.Accepted(data) };

    public static IServiceResult<T> NoContent(T? data)
        => new ServiceResult<T> { IsSuccess = true, Result = Result<T>.NoContent(data) };
}

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public sealed class PaginatedServiceResult<T, TPagination> : IPaginatedServiceResult<T, TPagination>
{
    [BindNever, Newtonsoft.Json.JsonIgnore, JsonIgnore]
    Result? IServiceResult.Result { get; }

    public bool IsSuccess { get; private init; }

    public Result<T, TPagination> Result { get; private init; }

    public ErrorResult? ErrorResult => null;

    public static IPaginatedServiceResult<T,TPagination> Ok((T Results, TPagination PaginationModel) data)
        => new PaginatedServiceResult<T,TPagination>
        {
            IsSuccess = true, 
            Result = Result<T, TPagination>.Ok(data.Results, data.PaginationModel)
        };

    public static IPaginatedServiceResult<T, PaginationModel> NoContent() 
        => new PaginatedServiceResult<T, PaginationModel>
        {
            IsSuccess = true, Result = Result<T,PaginationModel>.NoContent()
        };
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class Result
{
    public HttpStatus Status { get; private init; }

    internal static Result Ok()
        => new Result { Status = HttpStatus.Ok };

    internal static Result Created()
        => new Result { Status = HttpStatus.Created };

    internal static Result Accepted()
        => new Result { Status = HttpStatus.Accepted };

    internal static Result NoContent()
        => new Result { Status = HttpStatus.NoContent };
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class Result<T> where T : class
{
    public HttpStatus Status { get; private init; }

    public T? Data { get; private init; }

    internal static Result<T> Ok(T? data)
        => new Result<T> { Status = HttpStatus.Ok, Data = data };

    internal static Result<T> Created(T? data)
        => new Result<T> { Status = HttpStatus.Created, Data = data };

    internal static Result<T> Accepted(T? data)
        => new Result<T> { Status = HttpStatus.Accepted, Data = data };

    internal static Result<T> NoContent(T? data)
        => new Result<T> { Status = HttpStatus.NoContent, Data = data };
}

public sealed class Result<T, TPagination>
{
    public HttpStatus Status { get; private init; }

    public T? Data { get; private init; }

    public TPagination? PaginationModel { get; private init; }

    internal static Result<T, TPagination> Ok(T ? data, TPagination? paginationModel)
        => new Result<T, TPagination> { Status = HttpStatus.Ok, Data = data, PaginationModel = paginationModel };
    
    internal static Result<T, TPagination> NoContent()
        => new Result<T, TPagination> { Status = HttpStatus.NoContent, Data = default};
    
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class ErrorResult
{
    public HttpStatus Status { get; private init; }

    public ExceptionModel? Data { get; private init; }

    internal static ErrorResult SystemError(ExceptionModel? data)
        => new ErrorResult { Status = HttpStatus.Internal, Data = data };

    internal static ErrorResult GlobalSystemError()
        => SystemError(new ExceptionModel { Code = ExceptionConstants.System.InternalServerError, Message = ExceptionConstants.CommonErrorMessage });

    internal static ErrorResult UnauthorizedError()
        => new ErrorResult { Status = HttpStatus.Unauthorized, Data = new ExceptionModel { Code = ExceptionConstants.Business.UnauthorizedError } };

    internal static ErrorResult UnauthorizedError(ExceptionModel? data)
        => new ErrorResult { Status = HttpStatus.Unauthorized, Data = data };

    internal static ErrorResult BusinessError(ExceptionModel? data)
        => new ErrorResult { Status = HttpStatus.BadRequest, Data = data };

    internal static ErrorResult GlobalBusinessError()
        => BusinessError(new ExceptionModel { Code = ExceptionConstants.Business.BusinessInternalServerError, Message = ExceptionConstants.CommonErrorMessage });

    internal static ErrorResult NotFoundError()
        => new ErrorResult { Status = HttpStatus.NotFound, Data = new ExceptionModel { Code = ExceptionConstants.Business.NotFoundError } };
    
    internal static ErrorResult NotFoundError(ExceptionModel? data)
        => new ErrorResult { Status = HttpStatus.NotFound, Data = data };

    internal static ErrorResult AlreadyExistsError()
        => new ErrorResult { Status = HttpStatus.BadRequest, Data = new ExceptionModel { Code = ExceptionConstants.Business.AlreadyExistsError } };
    
    internal static ErrorResult AlreadyExistsError(ExceptionModel? data)
        => new ErrorResult { Status = HttpStatus.BadRequest, Data = data };

    internal static ErrorResult ForbiddenError()
        => new ErrorResult { Status = HttpStatus.Forbidden, Data = new ExceptionModel { Code = ExceptionConstants.Business.ForbiddenError } };
    
    internal static ErrorResult ForbiddenError(ExceptionModel? data)
        => new ErrorResult { Status = HttpStatus.Forbidden, Data = data };
    
    internal static ErrorResult BadRequest()
        => new ErrorResult { Status = HttpStatus.BadRequest, Data = new ExceptionModel { Code = ExceptionConstants.Business.ForbiddenError } };
    
    internal static ErrorResult BadRequest(ExceptionModel? data)
        => new ErrorResult { Status = HttpStatus.BadRequest, Data = data };
}

