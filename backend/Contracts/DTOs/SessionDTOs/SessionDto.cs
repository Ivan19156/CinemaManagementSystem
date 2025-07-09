using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.SessionDTOs
{
    public class SessionDto
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public decimal TicketPrice { get; set; }
    }

}
