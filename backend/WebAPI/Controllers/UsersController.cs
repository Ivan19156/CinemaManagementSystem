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
    using global::CinemaManagementSystem.Infrastructure.Logging;
    using Application.Authentication;
    using Microsoft.IdentityModel.JsonWebTokens;
    using System.Security.Claims;

    namespace CinemaManagementSystem.WebAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UsersController : ControllerBase
        {
            private readonly IUserService _userService;
            private readonly IAppLogger<UsersController> _logger;
            private readonly IJwtProvider _jwtProvider;

            public UsersController(IUserService userService, IAppLogger<UsersController> logger, IJwtProvider jwtProvider)
            {
                _userService = userService;
                _logger = logger;
                _jwtProvider = jwtProvider;

            }

            [HttpGet]
            [Authorize] // За потреби можна додати [Authorize(Roles = "admin")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<List<UserDto>>> GetAll()
            {
                _logger.LogInfo("Starting GetAll users");

                var result = await _userService.GetAllAsync();
                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"GetAll failed: {result.Message}");
                    return BadRequest(result.Message);
                }

                _logger.LogInfo($"GetAll succeeded: returned {result.Data.Count} users");
                return Ok(result.Data);
            }

            [HttpGet("{id}")]
            [Authorize(Policy = "ResourceOwnerOrAdmin")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<UserDto>> GetById(int id)
            {
                _logger.LogInfo($"GetById started for userId={id}");

                var result = await _userService.GetByIdAsync(id);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"GetById failed for userId={id}: {result.Message}");
                    return NotFound(result.Message);
                }

                _logger.LogInfo($"GetById succeeded for userId={id}");
                return Ok(result.Data);
            }

            [HttpPost("register")]
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<int>> Register(RegisterUserDto dto)
            {

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState is invalid");
                    return BadRequest(ModelState); // Повертає конкретні помилки валідації
                }

                _logger.LogInfo($"Register attempt for email={dto.Email}");

                var result = await _userService.RegisterAsync(dto);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"Register failed for email={dto.Email}: {result.Message}");
                    return BadRequest(result.Message);
                }

                _logger.LogInfo($"Register succeeded for userId={result.Data}");
                return Ok(result.Data);
            }

            [HttpPost("login")]
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<ActionResult<string>> Login(LoginDto dto)
            {
                _logger.LogInfo($"Login attempt for email={dto.Email}");

                var result = await _userService.LoginAsync(dto);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"Login failed for email={dto.Email}: {result.Message}");
                    return Unauthorized(result.Message);
                }
                var token = result.Data;
                Response.Cookies.Append("auth_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // ensure you're using HTTPS in production
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                _logger.LogInfo($"Login succeeded for email={dto.Email}");
                return Ok(result.Data);
            }

            [HttpPost("logout")]
            [Authorize] // Optional, depending on whether only authenticated users can call logout
            [ProducesResponseType(StatusCodes.Status200OK)]
            public IActionResult Logout()
            {
                // Remove the auth_token cookie by setting it with an expired date
                Response.Cookies.Append("auth_token", "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(-1) // Expire the cookie immediately
                });

                // Optionally log the action
                _logger.LogInfo("User logged out.");

                return Ok(new { message = "Logged out successfully." });
            }


            [HttpPut("{id}")]
            [Authorize(Policy = "ResourceOwnerOrAdmin")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult> Update(int id, UpdateUserDto dto)
            {
                _logger.LogInfo($"UpdateProfile started for userId={id}");

                var result = await _userService.UpdateProfileAsync(id, dto);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"UpdateProfile failed for userId={id}: {result.Message}");
                    return NotFound(result.Message);
                }

                _logger.LogInfo($"UpdateProfile succeeded for userId={id}");
                return NoContent();
            }

            [HttpDelete("{id}")]
            [Authorize(Policy = "ResourceOwnerOrAdmin")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult> Delete(int id)
            {
                _logger.LogInfo($"Delete user started for userId={id}");

                var result = await _userService.DeleteAsync(id);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"Delete user failed for userId={id}: {result.Message}");
                    return NotFound(result.Message);
                }

                _logger.LogInfo($"Delete user succeeded for userId={id}");
                return NoContent();
            }


            [HttpGet("me")]
            [Authorize]
            public async Task<IActionResult> Me()
            {
                var subClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (subClaim == null)
                    return Unauthorized("Token does not contain 'sub' claim");

                if (!int.TryParse(subClaim.Value, out var userId))
                    return Unauthorized("Invalid user ID in token");

                var result = await _userService.GetByIdAsync(userId);
                if (!result.IsSuccess)
                    return NotFound(result.Message);

                return Ok(result.Data);
            }


        }

    }
}
