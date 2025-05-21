using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.TicketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<ServiceResult<List<TicketDto>>> GetAllAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            var dtos = tickets.Select(t => new TicketDto
            {
                Id = t.Id,
                SessionId = t.SessionId,
                SeatNumber = t.SeatNumber,
                Price = t.Price
            }).ToList();
            return ServiceResult<List<TicketDto>>.Success(dtos);

        }

        public async Task<ServiceResult<TicketDto>> GetByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return ServiceResult<TicketDto>.Failure("Квиток не знайдено");

            var dto = new TicketDto
            {
                Id = ticket.Id,
                SessionId = ticket.SessionId,
                SeatNumber = ticket.SeatNumber,
                Price = ticket.Price
            };

            return ServiceResult<TicketDto>.Success(dto);
        }

        public async Task<ServiceResult<int>> CreateAsync(CreateTicketDto dto)
        {
            var ticket = new Ticket
            {
                SessionId = dto.SessionId,
                SeatNumber = dto.SeatNumber,
                Price = dto.Price
            };

            await _ticketRepository.AddAsync(ticket);
            return ServiceResult<int>.Success(ticket.Id);
        }

        public async Task<ServiceResult<bool>> UpdateAsync(int id, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return ServiceResult<bool>.Failure("Квиток не знайдено");

            ticket.SeatNumber = dto.SeatNumber;
            ticket.Price = dto.Price;

            await _ticketRepository.UpdateAsync(ticket);
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return ServiceResult<bool>.Failure("Квиток не знайдено");

            await _ticketRepository.DeleteAsync(ticket);
            return ServiceResult<bool>.Success(true);
        }
    }
}

