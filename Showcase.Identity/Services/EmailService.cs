using System.Net;
using System.Net.Mail;

namespace Showcase.Identity.Services;

public class EmailService(IConfiguration configuration)
{
    public async Task SendEmailAsync(string fromEmail, string toEmailAddress, string subject, string body)
    {
        var host = configuration["EmailSettings:Host"]; 
        var fromPassword = configuration["EmailSettings:FromPassword"];

        var smtpClient = new SmtpClient();
        
        smtpClient.Host = host!;
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Port = 587;
        smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);
        smtpClient.EnableSsl = true; 
        
        var message = new MailMessage();
        
        message.From = new MailAddress(fromEmail!);
        message.To.Add(toEmailAddress);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;
        
        await smtpClient.SendMailAsync(message);
    }
    
}