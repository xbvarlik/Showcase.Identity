using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Showcase.Identity.Data.Entities;

[Table("Role")]
public class Role : IdentityRole<Guid>
{
    // public virtual ICollection<UserRole> UserRoles {get; set; } = [];
}