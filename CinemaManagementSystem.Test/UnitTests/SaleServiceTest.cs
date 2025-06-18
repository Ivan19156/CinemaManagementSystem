using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure.Services;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.SaleDTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CinemaManagementSystem.Test.UnitTests;

public class SaleServiceTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly SaleService _saleService;

    public SaleServiceTests()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _saleService = new SaleService(_saleRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsSuccess_ReturnsMappedDtos()
    {
        // Arrange
        var sales = new List<Sale> { new Sale { Id = 1, UserId = 2, TicketsCount = 3, TotalPrice = 100, SaleDate = DateTime.Today } };
        _saleRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(ServiceResult<IEnumerable<Sale>>.Success(sales));

        // Act
        var result = await _saleService.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data);
        Assert.Equal(1, result.Data.First().Id);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsFailure_ReturnsFailure()
    {
        _saleRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(ServiceResult<IEnumerable<Sale>>.Failure("Error"));

        var result = await _saleService.GetAllAsync();

        Assert.False(result.IsSuccess);
        Assert.Equal("Error", result.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSaleExists_ReturnsMappedDto()
    {
        var sale = new Sale { Id = 1, UserId = 2, TicketsCount = 3, TotalPrice = 50, SaleDate = DateTime.Today };
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Sale>.Success(sale));

        var result = await _saleService.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSaleNotFound_ReturnsFailure()
    {
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Sale>.Failure("Not found"));

        var result = await _saleService.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("Not found", result.Message);
    }

    [Fact]
    public async Task CreateAsync_CreatesSaleSuccessfully()
    {
        var dto = new CreateSaleDto { UserId = 1, TicketsCount = 2, TotalPrice = 20, SaleDate = DateTime.Today };
        _saleRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Sale>()))
            .ReturnsAsync(ServiceResult<int>.Success(42));

        var result = await _saleService.CreateAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Data);
    }

    [Fact]
    public async Task UpdateAsync_WhenSaleExists_UpdatesSuccessfully()
    {
        var dto = new UpdateSaleDto { Id = 1, UserId = 2, TicketsCount = 3, TotalPrice = 30, SaleDate = DateTime.Today };
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Sale>.Success(new Sale { Id = 1 }));
        _saleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Sale>()))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        var result = await _saleService.UpdateAsync(dto);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateAsync_WhenSaleNotFound_ReturnsFailure()
    {
        var dto = new UpdateSaleDto { Id = 1, UserId = 2, TicketsCount = 3, TotalPrice = 30, SaleDate = DateTime.Today };
        _saleRepositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Sale>.Failure("Sale not found"));

        var result = await _saleService.UpdateAsync(dto);

        Assert.False(result.IsSuccess);
        Assert.Equal("Sale not found", result.Message);
    }

    [Fact]
    public async Task DeleteAsync_DeletesSuccessfully()
    {
        _saleRepositoryMock.Setup(r => r.DeleteAsync(1))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        var result = await _saleService.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
    }
}
