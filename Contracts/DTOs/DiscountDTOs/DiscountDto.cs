using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.DiscountDTOs
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public decimal Percentage { get; set; }  // Наприклад, 0.10 для 10%
        public string Description { get; set; } = null!;
    }

}
