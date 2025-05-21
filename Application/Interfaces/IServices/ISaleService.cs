using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.SaleDTOs;
namespace Application.Interfaces.Services
{
    public interface ISaleService
    {
        Task<ServiceResult<IEnumerable<SaleDto>>> GetAllAsync();
        Task<ServiceResult<SaleDto>> GetByIdAsync(int id);
        Task<ServiceResult<int>> CreateAsync(CreateSaleDto dto);
        Task<ServiceResult<bool>> UpdateAsync(UpdateSaleDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }

}
