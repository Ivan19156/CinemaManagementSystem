using Application.Interfaces.Common;
using Application.Interfaces.Services;
using Contracts.DTOs.TicketDTOs;
using Contracts.Events;
using MassTransit;
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
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IPublishEndpoint _publishEndpoint; // Added missing field declaration  

        public TicketsController(ITicketService ticketService, IPublishEndpoint publishEndpoint) // Updated constructor to inject IEventPublisher  
        {
            _ticketService = ticketService;
            _publishEndpoint = publishEndpoint;
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
        public async Task<ActionResult<int>> Create([FromBody] CreateTicketDto dto)
        {
            var result = await _ticketService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            var ticketId = result.Data;

            var ticketEvent = new TicketCreatedEvent
            {
                TicketId = ticketId,
                SessionId = dto.SessionId,
                UserId = dto.UserId,
                Hall = dto.Hall,
                Movie = dto.Movie,              // якщо є
                Time = dto.Time,                // якщо є
                SeatNumber = dto.SeatNumber,
                Price = dto.Price,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _publishEndpoint.Publish(ticketEvent);

            return Ok(ticketId);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var tickets = await _ticketService.GetByUserIdAsync(userId);
            if (!tickets.Any())
                return NotFound("Квитків для цього користувача не знайдено");

            return Ok(tickets);
        }
    }
}



    

