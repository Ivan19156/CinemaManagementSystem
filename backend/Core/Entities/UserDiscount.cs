using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserDiscount
    {public int UserId { get; set; }
        public User User { get; set; }
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
        public DateTime ExpirationDate { get; set; } // Date when the discount expires
        public bool IsUsed { get; set; } = false;// Indicates if the discount has been used
        public int Id { get; set; }
    }
}
