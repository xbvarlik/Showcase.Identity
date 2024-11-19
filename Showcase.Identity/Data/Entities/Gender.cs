using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Showcase.Identity.Data.Entities;

public class Gender
{
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public IEnumerable<GenderText> Texts { get; set; }
}

public class GenderText 
{
    public int Id { get; set; }

    [Required] 
    public string Text { get; set; } = null!;

    [Required] 
    public string LanguageId { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public bool IsDeleted { get; set; }
    
    [Required]
    public int GenderId { get; set; }
    
    [ForeignKey("GenderId")]
    public virtual Gender? Gender { get; set; }
}