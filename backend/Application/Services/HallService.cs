using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.HallDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Helpers;

namespace Application.Services;

public class HallService : IHallService
{
    private readonly IHallRepository _hallRepository;
    public HallService(IHallRepository hallRepository)
    {
        _hallRepository = hallRepository;
    }


    public async Task<ServiceResult<List<HallDto>>> GetAllAsync()
    {
        var halls = await _hallRepository.GetAllAsync();
        var dtos = halls.Select(h => new HallDto
        {
            Id = h.Id,
            SeatsCount = h.Seats
        });
        return ServiceResult<List<HallDto>>.Success(dtos.ToList());
    }
    public async Task<ServiceResult<HallDto?>> GetByIdAsync(int id)
    {
        var hall = await _hallRepository.GetByIdAsync(id);
        if (hall == null)
            return ServiceResult<HallDto?>.Failure("Зал не знайдено");
        var dto = new HallDto
        {
            Id = hall.Id,
            SeatsCount = hall.Seats
        };
        return ServiceResult<HallDto?>.Success(dto);
    }
    public async Task<ServiceResult<int>> CreateAsync(CreateHallDto dto)
    {
        var hall = new Hall
        {
            Seats = (int)dto.SeatsCount
        };
        var id = await _hallRepository.CreateAsync(hall);
        return ServiceResult<int>.Success(id);
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, UpdateHallDto dto)
    {
        var hall = await _hallRepository.GetByIdAsync(id);
        if (hall == null)
            return ServiceResult<bool>.Failure("Зал не знайдено");
        hall.Seats = (int)dto.SeatsCount;
        var result = await _hallRepository.UpdateAsync(hall);
        return ServiceResult<bool>.Success(result);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var hall = await _hallRepository.GetByIdAsync(id);
        if (hall == null)
            return ServiceResult<bool>.Failure("Зал не знайдено");
        var result = await _hallRepository.DeleteAsync(id);
        return ServiceResult<bool>.Success(result);
    }


}
