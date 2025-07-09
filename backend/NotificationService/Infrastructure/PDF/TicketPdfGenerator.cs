using Contracts.Events;
using QuestPDF.Fluent;

namespace NotificationService.Infrastructure.PDF;

public class TicketPdfGenerator
{
    public byte[] Generate(TicketCreatedEvent ticket)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Content().Column(col =>
                {
                    col.Item().Text("Квиток на фільм").FontSize(20).Bold().AlignCenter();
                    col.Item().Text($"Фільм: {ticket.Movie}");
                    col.Item().Text($"Місце: {ticket.SeatNumber}");
                    col.Item().Text($"Час: {ticket.Time}");
                    col.Item().Text($"Ціна: {ticket.Price} грн");
                });
            });
        });

        return document.GeneratePdf();
    }
}
