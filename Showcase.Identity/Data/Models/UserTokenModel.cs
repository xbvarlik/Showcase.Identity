using System.ComponentModel.DataAnnotations;

namespace Showcase.Identity.Data.Models;

public class UserTokenModel
{
    [StringLength(512)]
    public required string Token { get; set; } = null!;
    
    [StringLength(32)]
    public required string RefreshToken { get; set; } = null!;
    
    public required DateTime Expires { get; set; }
    
    public DateTime RefreshTokenExpires => Expires.AddHours(6);
    
    [StringLength(40)]
    public Guid ClientId { get; set; }
}

public class UserTokenViewModel
{
    
    public Guid UserId { get; set; }
    
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(200)]
    public string FullName { get; set; } = null!;
    
    //[MinLength(1, ErrorMessage = ExceptionConstants.Business.UserRoleMinLengthError)]
    public List<string> Roles { get; set; } = null!;
    
    public UserTokenModel AuthProperties { get; set; } = null!;
}