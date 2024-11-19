using Microsoft.EntityFrameworkCore;
using Showcase.Identity.Data.Constants;
using Showcase.Identity.Data.Context;
using Showcase.Identity.Data.Entities;
using Showcase.Identity.Data.Mappers;
using Showcase.Identity.Data.Models;
using Showcase.Identity.Exceptions;

namespace Showcase.Identity.Services;

public class UserRoleService(UserService userService, UserRoleServiceGuard userRoleServiceGuard, MssqlContext context)
{
    public bool CheckUserIsInRoleAsync(UserTokenViewModel model, string roleName)
    {
        return model.Roles.Contains(roleName);
    }
    
    public async Task AddUserRolesAsync(UserRoleModel model, CancellationToken cancellationToken = default)
    {
        var user = await GetUserAsync(model.UserId, cancellationToken) 
                   ?? throw new ShowcaseNotFoundException();

        await userRoleServiceGuard.CheckCreateDataAndThrowAsync(model, cancellationToken);
        
        foreach (var roleId in model.RoleIds)
        {
            var userRoleCreate = new UserRoleCreateModel
            {
                UserId = user.Id,
                RoleId = roleId
            };
            // var role = await GetRoleAsync(roleId, cancellationToken);
            //
            // await userService.AddToRoleAsync(user, role.Name!);
            var entity = userRoleCreate.ToEntity();
            context.UserRoles.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task AssignUserRoleAsync(User user, CancellationToken cancellationToken = default)
    {
        await userService.AddToRoleAsync(user, AuthConstants.UserRoles.User);
    }
    
    public async Task RemoveUserRolesAsync(UserRoleModel model, CancellationToken cancellationToken = default)
    {
        var user = await GetUserAsync(model.UserId, cancellationToken);
        
        foreach (var roleId in model.RoleIds) 
        {
            var role = await GetRoleAsync(roleId, cancellationToken);
    
            await userService.RemoveFromRoleAsync(user, role.Name!);
        }
    }
    
    private async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userService.FindByIdAsync(userId.ToString());
    
        if (user == null)
            throw new ShowcaseNotFoundException();
    
        return user;
    }
    
    private async Task<Role> GetRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
    
        if (role == null)
            throw new Exception();
    
        return role;
    }
}

public class UserRoleServiceGuard(MssqlContext context)
{
    public async Task CheckCreateDataAndThrowAsync(UserRoleModel model, CancellationToken cancellationToken = default)
    {
        await CheckHasDuplicationAndThrowAsync(model, cancellationToken);
    }

    private async Task CheckHasDuplicationAndThrowAsync(UserRoleModel model, CancellationToken cancellationToken)
    {
        bool isExists = false;

        try
        {
            isExists = await context.UserRoles.AnyAsync(x => x.UserId == model.UserId && model.RoleIds.Contains(x.RoleId), cancellationToken);
        }
        catch (Exception e)
        {
            //throw new ImplikaDatabaseException(e);
        }

        if (isExists)
            throw new Exception("Already exists");
    }
}