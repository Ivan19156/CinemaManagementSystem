using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.SessionDTOs;
using Contracts.DTOs.TicketDTOs;
using Shared.Helpers;

namespace Application.Interfaces.Services
{
    public interface ISessionService
    {
        Task<ServiceResult<List<SessionDto>>> GetAllAsync();

        Task<ServiceResult<SessionDto>> GetByIdAsync(int id);

        Task<ServiceResult<int>> CreateAsync(CreateSessionDto dto);

        Task<ServiceResult<bool>> UpdateAsync(int id, UpdateSessionDto dto);

        Task<ServiceResult<bool>> DeleteAsync(int id);

        Task<ServiceResult<List<SessionDto>>> GetByFilmIdAsync(int filmId);
    }
}
