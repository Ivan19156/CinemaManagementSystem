using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.HallDTOs;
using Contracts.DTOs.SaleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    public SaleService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<ServiceResult<IEnumerable<SaleDto>>> GetAllAsync()
    {
        var result = await _saleRepository.GetAllAsync();
        if (!result.IsSuccess)
            return ServiceResult<IEnumerable<SaleDto>>.Failure(result.Message);

        var dtos = result.Data.Select(MapToDto);
        return ServiceResult<IEnumerable<SaleDto>>.Success(dtos);
    }

    public async Task<ServiceResult<SaleDto>> GetByIdAsync(int id)
    {
        var result = await _saleRepository.GetByIdAsync(id);
        if (!result.IsSuccess)
            return ServiceResult<SaleDto>.Failure(result.Message);

        return ServiceResult<SaleDto>.Success(MapToDto(result.Data));
    }

    public async Task<ServiceResult<int>> CreateAsync(CreateSaleDto dto)
    {
        var sale = new Sale
        {
            UserId = dto.UserId,
            TicketsCount = dto.TicketsCount,
            TotalPrice = dto.TotalPrice,
            SaleDate = dto.SaleDate
            // Tickets додаються окремо або з іншим механізмом
        };

        return await _saleRepository.CreateAsync(sale);
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateSaleDto dto)
    {
        var existing = await _saleRepository.GetByIdAsync(dto.Id);
        if (!existing.IsSuccess)
            return ServiceResult<bool>.Failure("Sale not found");

        var sale = existing.Data;
        sale.UserId = dto.UserId;
        sale.TicketsCount = dto.TicketsCount;
        sale.TotalPrice = dto.TotalPrice;
        sale.SaleDate = dto.SaleDate;

        return await _saleRepository.UpdateAsync(sale);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        return await _saleRepository.DeleteAsync(id);
    }

    private SaleDto MapToDto(Sale sale)
    {
        return new SaleDto
        {
            Id = sale.Id,
            UserId = sale.UserId,
            TicketsCount = sale.TicketsCount,
            TotalPrice = sale.TotalPrice,
            SaleDate = sale.SaleDate,
            TicketIds = sale.Tickets?.Select(t => t.Id).ToList() ?? new List<int>()
        };
    }
}
