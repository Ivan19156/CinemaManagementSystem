using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.TicketDTOs;
using Shared.Helpers;

namespace Application.Interfaces.Services
{
    public interface ITicketService
    {
        Task<ServiceResult<List<TicketDto>>> GetAllAsync();
        Task<ServiceResult<TicketDto>> GetByIdAsync(int id);
        Task<ServiceResult<int>> CreateAsync(CreateTicketDto dto);
        Task<ServiceResult<bool>> UpdateAsync(int id, UpdateTicketDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<IEnumerable<TicketDto>> GetByUserIdAsync(int userId);
    }
}
