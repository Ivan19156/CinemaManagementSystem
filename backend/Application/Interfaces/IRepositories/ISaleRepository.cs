using Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Helpers;

namespace Application.Interfaces.Repositories
{
    public interface ISaleRepository
    {
        Task<ServiceResult<IEnumerable<Sale>>> GetAllAsync();
        Task<ServiceResult<Sale>> GetByIdAsync(int id);
        Task<ServiceResult<int>> CreateAsync(Sale sale);
        Task<ServiceResult<bool>> UpdateAsync(Sale sale);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
