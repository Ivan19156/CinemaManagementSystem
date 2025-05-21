using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs.HallDTOs
{
    public class CreateHallDto
    {
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public int SeatsCount { get; set; }
    }

}
