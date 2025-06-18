using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure.Services;
using Contracts.DTOs.HallDTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CinemaManagementSystem.Test.UnitTests;

public class HallServiceTests
{
    private readonly Mock<IHallRepository> _hallRepositoryMock;
    private readonly HallService _hallService;

    public HallServiceTests()
    {
        _hallRepositoryMock = new Mock<IHallRepository>();
        _hallService = new HallService(_hallRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfHallDtos()
    {
        var halls = new List<Hall> { new Hall { Id = 1, Seats = 100 } };
        _hallRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(halls);

        var result = await _hallService.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Single(result.Data);
        Assert.Equal(1, result.Data[0].Id);
        Assert.Equal(100, result.Data[0].SeatsCount);
    }

    [Fact]
    public async Task GetByIdAsync_WhenHallExists_ReturnsDto()
    {
        var hall = new Hall { Id = 1, Seats = 200 };
        _hallRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hall);

        var result = await _hallService.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data?.Id);
        Assert.Equal(200, result.Data?.SeatsCount);
    }

    [Fact]
    public async Task GetByIdAsync_WhenHallNotFound_ReturnsFailure()
    {
        _hallRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Hall?)null);

        var result = await _hallService.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Equal("Зал не знайдено", result.Message);
    }

    [Fact]
    public async Task CreateAsync_CreatesHall_ReturnsId()
    {
        var dto = new CreateHallDto { SeatsCount = 150 };
        _hallRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Hall>())).ReturnsAsync(5);

        var result = await _hallService.CreateAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Data);
    }

    [Fact]
    public async Task UpdateAsync_WhenHallExists_UpdatesAndReturnsSuccess()
    {
        var hall = new Hall { Id = 1, Seats = 100 };
        var dto = new UpdateHallDto { SeatsCount = 120 };

        _hallRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hall);
        _hallRepositoryMock.Setup(r => r.UpdateAsync(hall)).ReturnsAsync(true);

        var result = await _hallService.UpdateAsync(1, dto);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
        Assert.Equal(120, hall.Seats);
    }

    [Fact]
    public async Task UpdateAsync_WhenHallNotFound_ReturnsFailure()
    {
        var dto = new UpdateHallDto { SeatsCount = 120 };
        _hallRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Hall?)null);

        var result = await _hallService.UpdateAsync(1, dto);

        Assert.False(result.IsSuccess);
        Assert.Equal("Зал не знайдено", result.Message);
    }

    [Fact]
    public async Task DeleteAsync_WhenHallExists_DeletesAndReturnsSuccess()
    {
        var hall = new Hall { Id = 1, Seats = 100 };
        _hallRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hall);
        _hallRepositoryMock.Setup(r => r.DeleteAsync(hall)).ReturnsAsync(true);

        var result = await _hallService.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteAsync_WhenHallNotFound_ReturnsFailure()
    {
        _hallRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Hall?)null);

        var result = await _hallService.DeleteAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("Зал не знайдено", result.Message);
    }
}
