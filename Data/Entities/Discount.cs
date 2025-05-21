using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Core.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public decimal Percentage { get; set; }
        public ICollection<UserDiscount> UserDiscounts { get; set; } 
    }
}
