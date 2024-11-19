using Microsoft.AspNetCore.Mvc;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Result;
using Showcase.Identity.Services;

namespace Showcase.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(AccountService service, UserService userService) : ControllerBase
{
    [HttpPost("login/seller")]
    public async Task<IServiceResult> SellerLoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        var result = await service.LoginAsync(model, HttpContext.Request.Headers.UserAgent.ToString(), cancellationToken);
        
        return !result.Roles.Contains(AuthConstants.UserRoles.Seller) 
            ? ServiceResult.Unauthorized() 
            : ServiceResult<UserTokenViewModel>.Ok(result);
    }
    
    [HttpPost("login/consumer")]
    public async Task<IServiceResult> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
    {
        var result = await service.LoginAsync(model, HttpContext.Request.Headers.UserAgent.ToString(), cancellationToken);
        
        return !result.Roles.Contains(AuthConstants.UserRoles.Consumer) 
            ? ServiceResult.Unauthorized() 
            : ServiceResult<UserTokenViewModel>.Ok(result);
    }
    
    [HttpPost("logout")]
    public async Task<IServiceResult> LogoutAsync(LogoutModel model, CancellationToken cancellationToken = default)
    {
        await service.LogoutAsync(model, cancellationToken);
        return ServiceResult.Ok();
    }
    
    [HttpPost("refresh-token")]
    public async Task<IServiceResult<UserTokenViewModel>> RefreshTokenAsync(RefreshTokenCreateModel model, CancellationToken cancellationToken = default)
    {
        return ServiceResult<UserTokenViewModel>.Ok(await service.RefreshTokenAsync(model, cancellationToken));
    }

    [HttpPost("register-user")]
    public async Task<IServiceResult> RegisterUserAsync([FromForm] RegisterModel model, CancellationToken cancellationToken = default)
    {
        var result = await service.RegisterUserAsync(model, cancellationToken);
        return ServiceResult<UserTokenViewModel>.Created(result);
    }

    [HttpPut("reset-password")]
    public async Task<IServiceResult> ResetPasswordAsync([FromBody] string email,
        CancellationToken cancellationToken = default)
    {
        await service.SendResetPasswordOtpAsync(email, cancellationToken);
        return ServiceResult.Ok();
    }
    
    [HttpPatch("update-signalR-connection")]
    public async Task<IServiceResult> UpdateSignalRConnectionAsync([FromBody] SessionSignalRConnectionUpdateModel model,
        CancellationToken cancellationToken = default)
    {
        await service.UpdateSignalRConnectionAsync(model.SignalRConnectionId, cancellationToken);
        return ServiceResult.NoContent();
    }

    [HttpPost("send-otp")]
    public async Task<IServiceResult> SendOtpAsync([FromBody] string email,
        CancellationToken cancellationToken = default)
    {
        await service.SendVerificationOtpAsync(email, cancellationToken);
        return ServiceResult.Ok();
    }

    [HttpPost("verify-otp")]
    public async Task<IServiceResult> VerifyOtpAsync([FromBody] OtpControlModel model,
        CancellationToken cancellationToken = default)
    {
        var value = await service.VerifyOtpAsync(model, cancellationToken);
        
        if (!value) return ServiceResult.BadRequest();
        
        await userService.UpdateEmailConfirmedAsync(true, cancellationToken);
        return ServiceResult.Ok();
    }
    
    [HttpDelete]
    public async Task<IServiceResult> DeleteAccountAsync(CancellationToken cancellationToken = default)
    {
        await service.DeleteAccountAsync(cancellationToken);
        return ServiceResult.NoContent();
    }
}