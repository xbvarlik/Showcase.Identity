using Showcase.Identity.Data.Entities;
using Showcase.Identity.Data.Models;

namespace Showcase.Identity.Data.Mappers;

public static class UserRoleMapper
{
    public static UserRole ToEntity(this UserRoleCreateModel createModel)
    {
        return new UserRole
            {
                RoleId = createModel.RoleId,
                UserId = createModel.UserId
            };
    
    
    }
}