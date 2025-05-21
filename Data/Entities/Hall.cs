using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.Core.Enums;
namespace CinemaManagementSystem.Core.Entities
{
    public class Hall
    {
        public int Id { get; set; }
        public int Seats { get; set; }
        public HallType Type { get; set; }
        public ICollection<Session> Sessions { get; set; } 

    }
}
