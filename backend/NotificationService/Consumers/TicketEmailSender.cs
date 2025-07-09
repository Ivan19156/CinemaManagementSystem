using Contracts.Events;
using MassTransit;
using NotificationService.Application;
using NotificationService.Infrastructure.PDF;

namespace NotificationService.Consumers;

public class TicketEmailSenderConsumer : IConsumer<TicketCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<TicketEmailSenderConsumer> _logger;
    private readonly TicketPdfGenerator _ticketPdfGenerator;

    public TicketEmailSenderConsumer(IEmailService emailService, ILogger<TicketEmailSenderConsumer> logger, TicketPdfGenerator ticketPdfGenerator)
    {
        _emailService = emailService;
        _logger = logger;
        _ticketPdfGenerator = ticketPdfGenerator;
    }

    public async Task Consume(ConsumeContext<TicketCreatedEvent> context)
    {
        var ticket = context.Message;

        // Генеруємо PDF
        var pdfBytes = _ticketPdfGenerator.Generate(ticket);

        // Текст повідомлення
        var emailBody = $"Дивіться квиток у вкладенні. Фільм: {ticket.Movie}, місце: {ticket.SeatNumber}, час: {ticket.Time}";

        // Надсилаємо
        await _emailService.SendEmailWithAttachmentAsync(
            to: ticket.Email, // або вручну
            subject: "Ваш квиток у PDF",
            body: emailBody,
            attachmentBytes: pdfBytes,
            fileName: "ticket.pdf"
        );

        _logger.LogInformation("📧 Ticket PDF emailed to {UserId}", ticket.UserId);
    }

}

