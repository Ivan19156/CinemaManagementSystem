using Application.Interfaces.Repositories;
using Core.Entities;
using Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly CinemaDbContext _context;

        public DiscountRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<Discount>>> GetAllAsync()
        {
            var discounts = await _context.Discounts.ToListAsync();
            return ServiceResult<IEnumerable<Discount>>.Success(discounts);
        }

        public async Task<ServiceResult<Discount>> GetByIdAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
                return ServiceResult<Discount>.Failure("Discount not found");

            return ServiceResult<Discount>.Success(discount);
        }

        public async Task<ServiceResult<int>> CreateAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return ServiceResult<int>.Success(discount.Id);
        }

        public async Task<ServiceResult<bool>> UpdateAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            var affected = await _context.SaveChangesAsync();
            return ServiceResult<bool>.Success(affected > 0);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
                return ServiceResult<bool>.Failure("Discount not found");

            _context.Discounts.Remove(discount);
            var affected = await _context.SaveChangesAsync();
            return ServiceResult<bool>.Success(affected > 0);
        }

        public async Task<ServiceResult<int>> AssignToUserAsync(int userId, int discountId, DateTime expirationDate)
        {
            try
            {
                // Перевірка наявності знижки
                var discount = await _context.Discounts.FindAsync(discountId);
                if (discount == null)
                    return ServiceResult<int>.Failure("Discount not found");

                // Перевірка, чи вже призначено
                var exists = await _context.UserDiscounts
                    .AnyAsync(ud => ud.UserId == userId && ud.DiscountId == discountId && !ud.IsUsed);
                if (exists)
                    return ServiceResult<int>.Failure("User already has this discount assigned");

                var userDiscount = new UserDiscount
                {
                    UserId = userId,
                    DiscountId = discountId,
                    ExpirationDate = expirationDate,
                    IsUsed = false
                };

                _context.UserDiscounts.Add(userDiscount);
                await _context.SaveChangesAsync();

                return ServiceResult<int>.Success(userDiscount.Id);
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure("Error assigning discount: " + ex.Message);
            }
        }

        public async Task<ServiceResult<bool>> MarkAsUsedAsync(int userId, int discountId)
        {
            try
            {
                var userDiscount = await _context.UserDiscounts
                    .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DiscountId == discountId && !ud.IsUsed);

                if (userDiscount == null)
                    return ServiceResult<bool>.Failure("Discount not assigned or already used");

                userDiscount.IsUsed = true;
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure("Error marking discount as used: " + ex.Message);
            }
        }
        public async Task<ServiceResult<bool>> ExistsAsync(int userId)
        {
            try
            {
                var exists = await _context.Users.AnyAsync(u => u.Id == userId);
                return ServiceResult<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Error checking user existence: {ex.Message}");
            }
        }

    }

}
