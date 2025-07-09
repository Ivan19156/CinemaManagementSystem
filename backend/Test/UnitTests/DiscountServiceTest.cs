using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure.Services;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.DiscountDTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CinemaManagementSystem.Test.UnitTests;

public class DiscountServiceTests
{
    private readonly Mock<IDiscountRepository> _discountRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly DiscountService _service;

    public DiscountServiceTests()
    {
        _discountRepoMock = new Mock<IDiscountRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _service = new DiscountService(_discountRepoMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_WhenSuccess_ReturnsMappedDiscounts()
    {
        var discounts = new List<Discount>
        {
            new Discount { Id = 1, Percentage = 10 },
            new Discount { Id = 2, Percentage = 20 }
        };

        _discountRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(ServiceResult<IEnumerable<Discount>>.Success(discounts));

        var result = await _service.GetAllAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenFound_ReturnsMappedDiscount()
    {
        var discount = new Discount { Id = 1, Percentage = 15 };
        _discountRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Discount>.Success(discount));

        var result = await _service.GetByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(15, result.Data.Percentage);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ReturnsFailure()
    {
        _discountRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Discount>.Failure("Not found"));

        var result = await _service.GetByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("Not found", result.Message);
    }

    [Fact]
    public async Task CreateAsync_CreatesAndReturnsId()
    {
        var dto = new CreateDiscountDto { Percentage = 25 };
        _discountRepoMock.Setup(r => r.CreateAsync(It.IsAny<Discount>()))
            .ReturnsAsync(ServiceResult<int>.Success(5));

        var result = await _service.CreateAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Data);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_UpdatesSuccessfully()
    {
        var discount = new Discount { Id = 1, Percentage = 10 };
        var dto = new UpdateDiscountDto { Id = 1, Percentage = 30 };

        _discountRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Discount>.Success(discount));

        _discountRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Discount>()))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        var result = await _service.UpdateAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
        Assert.Equal(30, discount.Percentage);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ReturnsFailure()
    {
        var dto = new UpdateDiscountDto { Id = 1, Percentage = 30 };

        _discountRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(ServiceResult<Discount>.Failure("Discount not found"));

        var result = await _service.UpdateAsync(dto);

        Assert.False(result.IsSuccess);
        Assert.Equal("Discount not found", result.Message);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepositoryAndReturnsResult()
    {
        _discountRepoMock.Setup(r => r.DeleteAsync(1))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        var result = await _service.DeleteAsync(1);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
    }

    [Fact]
    public async Task AssignToUserAsync_WhenUserExists_AssignsSuccessfully()
    {
        _userRepoMock.Setup(r => r.ExistsAsync(1))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        _discountRepoMock.Setup(r => r.AssignToUserAsync(1, 2, It.IsAny<DateTime>()))
            .ReturnsAsync(ServiceResult<int>.Success(99));

        var result = await _service.AssignToUserAsync(1, 2, DateTime.Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(99, result.Data);
    }

    [Fact]
    public async Task AssignToUserAsync_WhenUserNotExists_ReturnsFailure()
    {
        _userRepoMock.Setup(r => r.ExistsAsync(1))
            .ReturnsAsync(ServiceResult<bool>.Failure("User not found"));

        var result = await _service.AssignToUserAsync(1, 2, DateTime.Now);

        Assert.False(result.IsSuccess);
        Assert.Equal("User not found", result.Message);
    }

    [Fact]
    public async Task MarkAsUsedAsync_CallsRepository()
    {
        _discountRepoMock.Setup(r => r.MarkAsUsedAsync(1, 2))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        var result = await _service.MarkAsUsedAsync(1, 2);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
    }
}

