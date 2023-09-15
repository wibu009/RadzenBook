using Microsoft.Extensions.Configuration;
using RadzenBook.Common.Settings;
using RadzenBook.Service.Interfaces.Infrastructure.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RadzenBook.Service.Implements.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    
    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailSettings = _configuration.GetSection("SendGridSettings").Get<SendGridSettings>();
        var client = new SendGridClient(emailSettings.Key);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(emailSettings.From, emailSettings.FromName),
            Subject = subject,
            PlainTextContent = htmlMessage,
            HtmlContent = htmlMessage
        };
        
        msg.AddTo(new EmailAddress(email));
        msg.SetClickTracking(false, false);
        
        await client.SendEmailAsync(msg);
    }
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage, string attachmentName, string type, byte[] attachment)
    {
        var emailSettings = _configuration.GetSection("SendGridSettings").Get<SendGridSettings>();
        var client = new SendGridClient(emailSettings.Key);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(emailSettings.From, emailSettings.FromName),
            Subject = subject,
            PlainTextContent = htmlMessage,
            HtmlContent = htmlMessage
        };
        
        msg.AddTo(new EmailAddress(email));
        msg.SetClickTracking(false, false);
        msg.AddAttachment(attachmentName, Convert.ToBase64String(attachment), type);
        
        await client.SendEmailAsync(msg);
    }
}