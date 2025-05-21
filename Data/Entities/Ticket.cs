using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.Core.Enums;
namespace CinemaManagementSystem.Core.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public TicketStatus Status { get; set; } // "Available", "Sold", "Cancelled"

    }
}
