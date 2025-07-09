using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Ticket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public int TicketId { get; set; }
    public int SessionId { get; set; }
    public int UserId { get; set; }
    public string Movie { get; set; } = null!;
    public DateTime Time { get; set; }
    public string SeatNumber { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
