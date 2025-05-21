using Application.Interfaces.Services;
using Contracts.DTOs.DiscountDTOs;
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
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DiscountDto>>> GetAll()
        {
            var result = await _discountService.GetAllAsync();
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DiscountDto>> GetById(int id)
        {
            var result = await _discountService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Create([FromBody] CreateDiscountDto dto)
        {
            var result = await _discountService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDiscountDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");

            var result = await _discountService.UpdateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpPost("{discountId}/assign/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> AssignToUser(int userId, int discountId, [FromQuery] DateTime expirationDate)
        {
            var result = await _discountService.AssignToUserAsync(userId, discountId, expirationDate);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost("{discountId}/markused/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MarkAsUsed(int userId, int discountId)
        {
            var result = await _discountService.MarkAsUsedAsync(userId, discountId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}

