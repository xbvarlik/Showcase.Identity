using System.ComponentModel.DataAnnotations;

namespace Showcase.Identity.Data.Models;

public class UserViewModel : BaseViewModel
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public string FullName { get; set; } = null!;
}

public class UserCreateModel : BaseCreateModel
{
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    
    public string UniversityEmail { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public int Birthdate { get; set; }
    
    public int CampusId { get; set; }
    
    public int UniversityStartYear { get; set; }
    
    public int GraduationYear { get; set; }
    
    public int DepartmentId { get; set; }
    
    public int GenderId { get; set; }
    
    public IFormFile VerificationDocument { get; set; }
}

public class UserUpdateModel : BaseUpdateModel
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public string? UniversityEmail { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    [Url]
    public string? ProfilePictureUrl { get; set; }
    
    public DateOnly? Birthdate { get; set; }
    
    public int? DepartmentId { get; set; }
    
    public int? CampusId { get; set; }
}

public class UserQueryFilterModel : BaseQueryFilterModel
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public string? UniversityEmail { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public DateOnly? Birthdate { get; set; }
    
    public int? DepartmentId { get; set; }
    
    public int? CampusId { get; set; }
}

public class UserProfileViewModel : BaseViewModel
{
    public string FirstName { get; set; } = null!;
}

public class UserFlagsModel
{
    public bool IsEmailConfirmed { get; set; }
    
    public bool IsTermsAndConditionsAccepted { get; set; }
}