using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.Core.Enums;

namespace CinemaManagementSystem.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; } // Consider hashing this
        public UserType Role { get; set; } // "Admin", "User", etc.
        public decimal Bonus { get; set; }

         
        public ICollection<Sale> Sales { get; set; }
        public ICollection <UserDiscount> UserDiscounts { get; set; }
    }
}
