using Application.Interfaces.Services;
using Contracts.DTOs.SaleDTOs;
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
        public class SalesController : ControllerBase
        {
            private readonly ISaleService _saleService;

            public SalesController(ISaleService saleService)
            {
                _saleService = saleService;
            }

            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<IActionResult> GetAll()
            {
                var result = await _saleService.GetAllAsync();
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return Ok(result.Data);
            }

            [HttpGet("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetById(int id)
            {
                var result = await _saleService.GetByIdAsync(id);
                if (!result.IsSuccess)
                    return NotFound(result.Message);

                return Ok(result.Data);
            }

            [HttpPost]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Create([FromBody] CreateSaleDto dto)
            {
                var result = await _saleService.CreateAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
            }

            [HttpPut("{id}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Update(int id, [FromBody] UpdateSaleDto dto)
            {
                if (id != dto.Id)
                    return BadRequest("ID mismatch.");

                var result = await _saleService.UpdateAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                return NoContent();
            }

            [HttpDelete("{id}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> Delete(int id)
            {
                var result = await _saleService.DeleteAsync(id);
                if (!result.IsSuccess)
                    return NotFound(result.Message);

                return NoContent();
            }
        }
    }

