using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IDiscountRepository
    {
        Task<ServiceResult<IEnumerable<Discount>>> GetAllAsync();
        Task<ServiceResult<Discount>> GetByIdAsync(int id);
        Task<ServiceResult<int>> CreateAsync(Discount discount);
        Task<ServiceResult<bool>> UpdateAsync(Discount discount);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<int>> AssignToUserAsync(int userId, int discountId, DateTime expirationDate);
        Task<ServiceResult<bool>> MarkAsUsedAsync(int userId, int discountId);
        

    }

}
