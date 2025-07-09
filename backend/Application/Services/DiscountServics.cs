using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.DiscountDTOs;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IUserRepository _userRepository; // якщо треба перевіряти наявність юзера




    public DiscountService(IDiscountRepository discountRepository, IUserRepository userRepository)
    {
        _discountRepository = discountRepository;
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<IEnumerable<DiscountDto>>> GetAllAsync()
    {
        var result = await _discountRepository.GetAllAsync();
        if (!result.IsSuccess)
            return ServiceResult<IEnumerable<DiscountDto>>.Failure(result.Message);

        var dtos = result.Data.Select(MapToDto);
        return ServiceResult<IEnumerable<DiscountDto>>.Success(dtos);
    }

    public async Task<ServiceResult<DiscountDto>> GetByIdAsync(int id)
    {
        var result = await _discountRepository.GetByIdAsync(id);
        if (!result.IsSuccess)
            return ServiceResult<DiscountDto>.Failure(result.Message);

        return ServiceResult<DiscountDto>.Success(MapToDto(result.Data));
    }

    public async Task<ServiceResult<int>> CreateAsync(CreateDiscountDto dto)
    {
        var discount = new Discount
        {
            
            Percentage = (decimal)dto.Percentage,
           
        };

        return await _discountRepository.CreateAsync(discount);
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateDiscountDto dto)
    {
        var existing = await _discountRepository.GetByIdAsync(dto.Id);
        if (!existing.IsSuccess)
            return ServiceResult<bool>.Failure("Discount not found");

        var discount = existing.Data;
        
        discount.Percentage = (decimal)dto.Percentage;
        

        return await _discountRepository.UpdateAsync(discount);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        return await _discountRepository.DeleteAsync(id);
    }

    public async Task<ServiceResult<int>> AssignToUserAsync(int userId, int discountId, DateTime expirationDate)
    {
        // optionally check if user exists
        var userExists = await _userRepository.ExistsAsync(userId);
        if (!userExists.IsSuccess)
            return ServiceResult<int>.Failure("User not found");

        var result = await _discountRepository.AssignToUserAsync(userId, discountId, expirationDate);
        return result;
    }

    public async Task<ServiceResult<bool>> MarkAsUsedAsync(int userId, int discountId)
    {
        return await _discountRepository.MarkAsUsedAsync(userId, discountId);
    }

    private DiscountDto MapToDto(Discount discount)
    {
        return new DiscountDto
        {
            Id = discount.Id,
            Percentage = discount.Percentage,
            
        };
    }

}
