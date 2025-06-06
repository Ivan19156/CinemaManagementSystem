using System.ComponentModel.DataAnnotations;

namespace Contracts.DTOs.UsersDto
{
    // RegisterUserDto - для реєстрації користувача
    public class RegisterUserDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }


}
