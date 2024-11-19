using System.ComponentModel.DataAnnotations;

namespace Showcase.Identity.Data.Models;

public class LoginModel
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}

public record LogoutModel(string Email, Guid ClientId);

// public class RegisterModel
// {
//     [Required]
//     public string Email { get; set; } = null!;
//     
//     [Required]
//     public string Password { get; set; } = null!;
//     
//     [Required]
//     public string ConfirmPassword { get; set; } = null!;
// }

public class RegisterModel
{
    [Required]
    public string FullName { get; set; } = null!;
    
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
    
    [Required]
    public string PhoneNumber { get; set; } = null!;
    
    [Required]
    public int GenderId { get; set; }
    
    [Required]
    public int DepartmentId { get; set; }
    
    [Required]
    public int CampusId { get; set; }
    
    [Required]
    public IFormFile VerificationDocument { get; set; }
    
    public int UniversityStartYear { get; set; }
    
    public int GraduationYear { get; set; }
    
    public int Birthdate { get; set; }
}

public class AccountModel
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;
}