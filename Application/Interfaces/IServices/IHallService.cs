using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.HallDTOs;
namespace Application.Interfaces.Services
{
    public interface IHallService
    {
        Task<ServiceResult<List<HallDto>>> GetAllAsync();

        Task<ServiceResult<HallDto?>> GetByIdAsync(int id);

        Task<ServiceResult<int>> CreateAsync(CreateHallDto dto);

        Task<ServiceResult<bool>> UpdateAsync(int id, UpdateHallDto dto);

        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
