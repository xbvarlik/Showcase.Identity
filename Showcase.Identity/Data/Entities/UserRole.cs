using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Showcase.Identity.Data.Entities;

[Table("UserRole")]
[PrimaryKey("UserId", "RoleId")]
public class UserRole : IdentityUserRole<Guid>;

[Table("UserClaim")]
public class UserClaim : IdentityUserClaim<Guid>;

[Table("UserLogin")]
public class UserLogin : IdentityUserLogin<Guid>;

[Table("UserToken")]
public class UserToken : IdentityUserToken<Guid>;

[Table("RoleClaim")]
public class RoleClaim : IdentityRoleClaim<Guid>;