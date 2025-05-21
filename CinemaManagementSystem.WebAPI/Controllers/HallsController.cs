using Application.Interfaces.Services;
using Contracts.DTOs.HallDTOs;
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
    public class HallsController : ControllerBase
    {
        private readonly IHallService _hallService;
        public HallsController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<HallDto>>> GetAll()
        {
            var result = await _hallService.GetAllAsync();
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HallDto>> GetById(int id)
        {
            var result = await _hallService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateHallDto dto)
        {
            var result = await _hallService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateHallDto dto)
        {
            var result = await _hallService.UpdateAsync(id, dto);
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
            var result = await _hallService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return NoContent();
        }

       

    }
}
