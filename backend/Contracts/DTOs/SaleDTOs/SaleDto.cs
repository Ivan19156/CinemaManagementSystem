using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.SaleDTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public decimal Amount { get; set; }
        public DateTime SaleDate { get; set; }
        public int TicketsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<int> TicketIds { get; set; }
    }

}
