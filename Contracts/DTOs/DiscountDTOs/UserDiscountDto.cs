using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.DiscountDTOs
{
    public class UserDiscountDto
    {
        public int UserId { get; set; }
        public int DiscountId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
    }

}
