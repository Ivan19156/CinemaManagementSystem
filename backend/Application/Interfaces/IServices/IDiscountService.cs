using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.DiscountDTOs;
using Shared.Helpers;
namespace Application.Interfaces.Services
{
    public interface IDiscountService
    {
        Task<ServiceResult<IEnumerable<DiscountDto>>> GetAllAsync();
        Task<ServiceResult<DiscountDto>> GetByIdAsync(int id);
        Task<ServiceResult<int>> CreateAsync(CreateDiscountDto dto);
        Task<ServiceResult<bool>> UpdateAsync(UpdateDiscountDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);

        Task<ServiceResult<int>> AssignToUserAsync(int userId, int discountId, DateTime expirationDate);
        Task<ServiceResult<bool>> MarkAsUsedAsync(int userId, int discountId);

    }
}
