using Application.Interfaces.Services;
using Contracts.DTOs.SessionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<SessionDto>>> GetAll()
        {
            var result = await _sessionService.GetAllAsync();
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessionDto>> GetById(int id)
        {
            var result = await _sessionService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateSessionDto dto)
        {
            var result = await _sessionService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
        }
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateSessionDto dto)
        {
            var result = await _sessionService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sessionService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return NoContent();
        }
        [HttpGet("film/{filmId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<SessionDto>>> GetByFilmId(int filmId)
        {
            var result = await _sessionService.GetByFilmIdAsync(filmId);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
