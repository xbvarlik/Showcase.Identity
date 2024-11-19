using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Showcase.Identity.Data.Entities;

[Table("User")]
public class User : IdentityUser<Guid>
{
    public User()
    {
        Age = DateTime.Now.Year - Birthdate.Year;
    }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;
    
    [StringLength(250)]
    public string FullName => $"{FirstName} {LastName}";
    
    [Phone]
    [StringLength(13)]
    public string Phone { get; set; } = null!;
    
    public DateOnly Birthdate { get; set; }
    
    [NotMapped]
    public int Age { get; set; } 
    
    [Required]
    public int GenderId { get; set; }
    
    [ForeignKey("GenderId")]
    public virtual Gender? Gender { get; set; }
    
    public bool IsTermsAndConditionsAccepted { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ChangedAt { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public Guid? ChangedBy { get; set; }
    
    public bool IsDeleted { get; set; }
}