using RadzenBook.Application.Common.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RadzenBook.Infrastructure.Mail.SendGrid;

public class SendGridEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SendGridEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailSettings = _configuration.GetSection("SendGridSettings").Get<SendGridSettings>();
        var client = new SendGridClient(emailSettings!.Key);
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

    public async Task SendEmailAsync(string email, string subject, string htmlMessage, string attachmentName,
        string type, byte[] attachment)
    {
        var emailSettings = _configuration.GetSection("SendGridSettings").Get<SendGridSettings>();
        var client = new SendGridClient(emailSettings!.Key);
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