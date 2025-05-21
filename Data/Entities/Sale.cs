using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Core.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int TicketsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SaleDate { get; set; }

        public ICollection<Ticket> Tickets { get; set; } 
    }
}
