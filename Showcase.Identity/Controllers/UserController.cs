using Microsoft.AspNetCore.Mvc;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Result;
using Showcase.Identity.Services;

namespace Showcase.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService service, UserRoleService userRoleService, UserCredentials credentials) : ControllerBase
{
    
    [HttpGet]
    public async Task<IPaginatedServiceResult<List<UserViewModel>, PaginationModel>> GetAllAsync([FromQuery] UserQueryFilterModel filter, CancellationToken cancellationToken = default)
        => PaginatedServiceResult<List<UserViewModel>, PaginationModel>.Ok(await service.GetAllAsync(filter, cancellationToken));
    
    [HttpGet("{id}")]
    public async Task<IServiceResult<UserProfileViewModel>> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await service.GetUserProfileAsync(id, cancellationToken);
        
        return entity == null 
            ? ServiceResult<UserProfileViewModel>.NoContent(null) 
            : ServiceResult<UserProfileViewModel>.Ok(entity);
    }
    
    [HttpPost]
    public async Task<IServiceResult> CreateAsync([FromBody] UserCreateModel model, CancellationToken cancellationToken = default)
    {
        await service.CreateAsync(model, cancellationToken);
        return ServiceResult.Created();
    }
    
    [HttpPatch("{id}")]
    public async Task<IServiceResult> UpdateAsync([FromRoute] Guid id, [FromBody] UserUpdateModel model, CancellationToken cancellationToken = default)
    {
        await service.UpdateAsync(id, model, cancellationToken);
        return ServiceResult.NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IServiceResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await service.DeleteAsync(id, cancellationToken);
        return ServiceResult.NoContent();
    }

    [HttpGet("account")]
    public async Task<IServiceResult> GetUserAccountAsync(CancellationToken cancellationToken = default)
    {
        var account = await service.GetAccountVariablesAsync(cancellationToken);
        return account != null ? ServiceResult<AccountModel>.Ok(account) : ServiceResult.NotFound();
    }

    [HttpGet("profile")]
    public async Task<IServiceResult> GetUserProfileAsync(CancellationToken cancellationToken = default)
    {
        var user = await service.GetUserProfileAsync(credentials.UserId, cancellationToken);
        return user != null ? ServiceResult<UserProfileViewModel>.Ok(user) : ServiceResult.NotFound();
    }
    
    [HttpGet("user-flags")]
    public async Task<IServiceResult> GetUserFlags(CancellationToken cancellationToken = default)
    {
        var flags = await service.GetUserFlags(credentials.UserId, cancellationToken);
        return flags != null ? ServiceResult<UserFlagsModel>.Ok(flags) : ServiceResult.NotFound();
    }
    
    [HttpPatch("confirm/Email")]
    public async Task<IServiceResult> ConfirmUserEmail(CancellationToken cancellationToken = default)
    {
        await service.UpdateEmailConfirmedAsync(true, cancellationToken);
        return ServiceResult.NoContent();
    }
    
    [HttpPatch("accept/TermsAndConditions")]
    public async Task<IServiceResult> AcceptTermsAndConditions(CancellationToken cancellationToken = default)
    {
        await service.UpdateTermsAndConditionsAcceptedAsync(true, cancellationToken);
        return ServiceResult.NoContent();
    }
}