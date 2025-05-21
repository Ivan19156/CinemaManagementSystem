using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.TicketDTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string SeatNumber { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }
        public bool IsCanceled { get; set; }
        public decimal Price { get; set; }
    }

}
