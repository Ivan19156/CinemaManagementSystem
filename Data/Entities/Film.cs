using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Core.Entities
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Jenre { get; set; }
        public string Director { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int? AgeRestriction { get; set; } 
        public string Description { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
