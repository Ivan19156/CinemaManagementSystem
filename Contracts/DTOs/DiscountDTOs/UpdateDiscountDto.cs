using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.DiscountDTOs
{
    public class UpdateDiscountDto : CreateDiscountDto
    {
        public int Id { get; set; }
    }
}
