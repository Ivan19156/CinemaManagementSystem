using Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Consumers;

public class TicketCreatedConsumer : IConsumer<TicketCreatedEvent>
{
    private readonly ITicketRepository _repository;
    private readonly ILogger<TicketCreatedConsumer> _logger;

    public TicketCreatedConsumer(ITicketRepository repository, ILogger<TicketCreatedConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TicketCreatedEvent> context)
    {
        var e = context.Message;

        if (await _repository.ExistsAsync(e.TicketId))
        {
            _logger.LogWarning("Ticket with ID={TicketId} already exists. Skipping...", e.TicketId);
            return;
        }

        var ticket = new Ticket
        {
            TicketId = e.TicketId,
            SessionId = e.SessionId,
            UserId = e.UserId,
            Movie = e.Movie,
            Time = e.Time,
            SeatNumber = e.SeatNumber,
            Price = e.Price,
            CreatedAt = e.CreatedAt
        };

        try
        {
            await _repository.AddAsync(ticket);
            _logger.LogInformation("Ticket stored in MongoDB. ID={TicketId}", e.TicketId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to store ticket in MongoDB. ID={TicketId}", e.TicketId);
            throw; // щоб виклик вищого рівня знав про помилку
        }
    }

}
