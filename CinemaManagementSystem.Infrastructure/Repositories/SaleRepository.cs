using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly CinemaDbContext _context;
        public SaleRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<Sale>>> GetAllAsync()
        {
            var sales = await _context.Sales.ToListAsync();
            return ServiceResult<IEnumerable<Sale>>.Success(sales);
        }

        public async Task<ServiceResult<Sale>> GetByIdAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
                return ServiceResult<Sale>.Failure("Sale not found");

            return ServiceResult<Sale>.Success(sale);
        }

        public async Task<ServiceResult<int>> CreateAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return ServiceResult<int>.Success(sale.Id);
        }

        public async Task<ServiceResult<bool>> UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            var updated = await _context.SaveChangesAsync() > 0;
            return updated
                ? ServiceResult<bool>.Success(true)
                : ServiceResult<bool>.Failure("Update failed");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
                return ServiceResult<bool>.Failure("Sale not found");

            _context.Sales.Remove(sale);
            var deleted = await _context.SaveChangesAsync() > 0;
            return deleted
                ? ServiceResult<bool>.Success(true)
                : ServiceResult<bool>.Failure("Delete failed");
        }
    }
}
