using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events;

public class TicketCreatedEvent
{
    public int TicketId { get; set; }
    public int SessionId { get; set; }
    public int UserId { get; set; }
    public string Movie { get; set; }
    public DateTime Time { get; set; }
    public string SeatNumber { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Hall { get; set; }
    public string Email { get; set; }
}

