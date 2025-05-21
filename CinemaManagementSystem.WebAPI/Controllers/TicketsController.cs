using Application.Interfaces.Services;
using Contracts.DTOs.TicketDTOs;
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
    public class TicketsContrloller : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketsContrloller(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<TicketDto>>> GetAll()
        {
            var result = await _ticketService.GetAllAsync();
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketDto>> GetById(int id)
        {
            var result = await _ticketService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result.Data);
        }
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateTicketDto dto)
        {
            var result = await _ticketService.UpdateAsync(id, dto);
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
            var result = await _ticketService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Create(CreateTicketDto dto)
        {
            var result = await _ticketService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data); // Повертає ID створеного квитка
        }



    }
}
