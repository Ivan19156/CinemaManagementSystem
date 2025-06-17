using System.ComponentModel.DataAnnotations;

namespace Contracts.DTOs.UsersDto
{
    // RegisterUserDto - для реєстрації користувача
    public class RegisterUserDto
    {
       
        public string Name { get; set; }

        
        public string Email { get; set; }

        public string Password { get; set; }
    }


}
