namespace Showcase.Identity.Settings;

public class JwtBearerTokenSettings
{
    public string PrimarySecretKey { get; set; } = null!;
    
    public string SecondarySecretKey { get; set; } = null!;
    
    public string Audience { get; set; } = null!;
    
    public string Issuer { get; set; } = null!;
    
    public int ExpiryTimeInSeconds { get; set; }
}