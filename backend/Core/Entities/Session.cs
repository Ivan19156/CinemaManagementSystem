using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
namespace Core.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
        public SessionStatus Status { get; set; } // "Available", "Sold", "Cancelled"
        public ICollection<Ticket> Tickets { get; set; }

    }
}
