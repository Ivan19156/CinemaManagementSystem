namespace NotificationService.Application;

public interface IEmailService
{
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string fileName);
}
