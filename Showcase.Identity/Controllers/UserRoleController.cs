using Microsoft.AspNetCore.Mvc;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Result;
using Showcase.Identity.Services;

namespace Showcase.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController(UserRoleService userRoleService) : ControllerBase
{
    [HttpPost]
    public async Task<IServiceResult> CreateAsync(UserRoleModel model, CancellationToken cancellationToken = default)
    {
        await userRoleService.AddUserRolesAsync(model, cancellationToken);
        return ServiceResult.Ok();
    }
    
    [HttpDelete]
    public async Task<IServiceResult> DeleteAsync(UserRoleModel model, CancellationToken cancellationToken = default)
    {
        await userRoleService.RemoveUserRolesAsync(model, cancellationToken);
        return ServiceResult.Ok();
    }
}