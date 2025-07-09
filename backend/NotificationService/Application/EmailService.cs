using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NotificationService.Application;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string fileName)
    {
        var message = new MimeMessage();
        var from = _configuration["Email:SmtpUser"];
        message.From.Add(MailboxAddress.Parse(from));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = body
        };

        builder.Attachments.Add(fileName, attachmentBytes, new ContentType("application", "pdf"));
        message.Body = builder.ToMessageBody();

        var host = _configuration["Email:SmtpHost"];
        var port = int.Parse(_configuration["Email:SmtpPort"]);
        var user = _configuration["Email:SmtpUser"];
        var pass = _configuration["Email:SmtpPass"];

        using var client = new SmtpClient();

        // Gmail вимагає SSL-підключення на порт 465
        await client.ConnectAsync(host, port, SecureSocketOptions.SslOnConnect);
        await client.AuthenticateAsync(user, pass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}



