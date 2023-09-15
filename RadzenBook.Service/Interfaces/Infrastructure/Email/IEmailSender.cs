namespace RadzenBook.Service.Interfaces.Infrastructure.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);

    Task SendEmailAsync(string email, string subject, string htmlMessage, string attachmentName, string type,
        byte[] attachment);
}