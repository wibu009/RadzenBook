namespace RadzenBook.Application.Common.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);

    Task SendEmailAsync(string email, string subject, string htmlMessage, string attachmentName, string type,
        byte[] attachment);
}