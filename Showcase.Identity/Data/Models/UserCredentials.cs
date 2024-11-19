namespace Showcase.Identity.Data.Models;

public class UserCredentials
{
    public Guid UserId { get; set; }
    
    public int CampusId { get; set; }
    
    public List<string> UserRoles { get; set; } = [];
    
    public string? LanguageId { get; set; }
}