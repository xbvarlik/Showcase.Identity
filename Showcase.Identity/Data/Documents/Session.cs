using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Showcase.Identity.Data.Models;

namespace Showcase.Identity.Data.Documents;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Session
{
    [StringLength(40)]
    public Guid UserId { get; set; }
    
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;

    [StringLength(512)]
    public required string Agent { get; set; } = null!;
    
    public required Guid ClientId { get; set; }
    
    [StringLength(128)]
    public required string Email { get; set; } = null!;

    public int CampusId { get; set; } 
    
    public string? SignalRConnectionId { get; set; }
    
    [StringLength(128)]
    public required string NormalizedEmail { get; set; } = null!;
    
    //[MinLength(1, ErrorMessage = ExceptionConstants.Business.UserRoleMinLengthError)]
    public required IList<string> UserRoles { get; set; } = null!;
    
    public required UserTokenModel AuthProperties { get; set; } = null!;
    
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public required DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= AuthProperties.RefreshTokenExpires;
}