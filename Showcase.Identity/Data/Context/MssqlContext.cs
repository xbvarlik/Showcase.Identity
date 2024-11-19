using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Showcase.Identity.Data.Entities;
using Role = Showcase.Identity.Data.Entities.Role;
using UserClaim = Showcase.Identity.Data.Entities.UserClaim;

namespace Showcase.Identity.Data.Context;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class MssqlContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public MssqlContext()
    {
        
    }

    public MssqlContext(DbContextOptions<MssqlContext> options) : base(options)
    {
        
    }
    

    
    // Customizing
    public virtual DbSet<Gender> Gender { get; set; } = null!;
    
    // Multiplexers
    public virtual DbSet<UserRole> UserRole { get; set; } = null!;
    
    // Localization
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Seed();
        base.OnModelCreating(builder);
    }
}