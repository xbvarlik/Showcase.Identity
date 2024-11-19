namespace Showcase.Identity.Data.Models;

public class RefreshTokenViewModel
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Token { get; set; } = null!;
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime? Revoked { get; set; }
    
    public string? ReplacedByToken { get; set; }
    
    public bool IsActive => Revoked == null && !IsExpired;
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}

public record RefreshTokenCreateModel(string Email, Guid ClientId);