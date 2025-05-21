using Application.Interfaces.Services;
using Contracts.DTOs.UsersDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CinemaManagementSystem.WebAPI.Controllers
{
    using Application.Interfaces.Services;
    using Contracts.DTOs.UsersDto;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;

    namespace CinemaManagementSystem.WebAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UsersController : ControllerBase
        {
            private readonly IUserService _userService;

            public UsersController(IUserService userService)
            {
                _userService = userService;
            }

            // Отримати всіх користувачів — тільки для авторизованих (можна додати роль, якщо потрібно)
            [HttpGet]
            [Authorize] // За потреби можна додати [Authorize(Roles = "admin")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<List<UserDto>>> GetAll()
            {
                var result = await _userService.GetAllAsync();
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return Ok(result.Data);
            }

            // Отримати користувача за ID — власник або адмін
            [HttpGet("{id}")]
            [Authorize(Policy = "ResourceOwnerOrAdmin")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<UserDto>> GetById(int id)
            {
                var result = await _userService.GetByIdAsync(id);
                if (!result.IsSuccess)
                    return NotFound(result.Message);

                return Ok(result.Data);
            }

            // Реєстрація — публічний доступ
            [HttpPost("register")]
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<int>> Register(RegisterUserDto dto)
            {
                var result = await _userService.RegisterAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return Ok(result.Data);
            }

            // Логін — публічний доступ
            [HttpPost("login")]
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<ActionResult<string>> Login(LoginDto dto)
            {
                var result = await _userService.LoginAsync(dto);
                if (!result.IsSuccess)
                    return Unauthorized(result.Message);

                return Ok(result.Data);
            }

            // Оновлення профілю — власник або адмін
            [HttpPut("{id}")]
            [Authorize(Policy = "ResourceOwnerOrAdmin")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult> Update(int id, UpdateUserDto dto)
            {
                var result = await _userService.UpdateProfileAsync(id, dto);
                if (!result.IsSuccess)
                    return NotFound(result.Message);

                return NoContent();
            }

            // Видалення користувача — власник або адмін
            [HttpDelete("{id}")]
            [Authorize(Policy = "ResourceOwnerOrAdmin")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult> Delete(int id)
            {
                var result = await _userService.DeleteAsync(id);
                if (!result.IsSuccess)
                    return NotFound(result.Message);

                return NoContent();
            }
        }
    }

}
