namespace Showcase.Identity.Data.Models;

public class OtpControlModel
{
    public string Email { get; set; } = null!;
    
    public int OtpCode { get; set; }
}