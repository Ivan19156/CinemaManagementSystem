using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.DTOs.UsersDto;
namespace Application.Interfaces.Services
{
    
    //using CinemaWebApi.DTOs;
    //using CinemaWebApi.Helpers;
    using CinemaManagementSystem.WebAPI.Helpers;
    using Shared.Helpers;

    public interface IUserService
    {
        Task<ServiceResult<List<UserDto>>> GetAllAsync();
        Task<ServiceResult<UserDto?>> GetByIdAsync(int id);
        Task<ServiceResult<int>> RegisterAsync(RegisterUserDto dto);
        Task<ServiceResult<string>> LoginAsync(LoginDto dto);
        Task<ServiceResult<bool>> UpdateProfileAsync(int id, UpdateUserDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<int>> RegisterAdminAsync(RegisterUserDto dto);
    }

}
