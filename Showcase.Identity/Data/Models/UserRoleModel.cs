namespace Showcase.Identity.Data.Models;

public class UserRoleViewModel
{
    public Guid UserId { get; set; }
    
    public Guid RoleId { get; set; }
}

public class UserRoleModel
{
    public Guid UserId { get; set; }
    
    public List<Guid> RoleIds { get; set; }
}
public class UserRoleCreateModel
{
    public Guid UserId { get; set; }
    
    public Guid RoleId { get; set; }
}