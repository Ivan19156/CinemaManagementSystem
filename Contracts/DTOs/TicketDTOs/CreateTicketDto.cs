using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.TicketDTOs
{
    public class CreateTicketDto
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string SeatNumber { get; set; } = null!;
        public decimal Price { get; set; }
    }

}
