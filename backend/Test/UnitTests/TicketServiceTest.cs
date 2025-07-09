using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure.Services;
using Contracts.DTOs.TicketDTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CinemaManagementSystem.Test.UnitTests;

public class TicketServiceTest
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly TicketService _ticketService;
    public TicketServiceTest()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _ticketService = new TicketService(_ticketRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var tickets = new List<CinemaManagementSystem.Core.Entities.Ticket>
        {
            new CinemaManagementSystem.Core.Entities.Ticket { Id = 1, SessionId = 1, SeatNumber = "A1", Price = 100 },
            new CinemaManagementSystem.Core.Entities.Ticket { Id = 2, SessionId = 1, SeatNumber = "A2", Price = 100 }
        };
        _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tickets);
        // Act
        var result = await _ticketService.GetAllAsync();
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoTickets_ReturnsEmptyList()
    {

        _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Ticket>());

        var res = await _ticketService.GetAllAsync();
        Assert.True(res.IsSuccess);
        Assert.Empty(res.Data);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsTicket()
    {
        // Arrange
        var ticket = new Ticket { Id = 1, SessionId = 1, SeatNumber = "A1", Price = 100 };
        _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ticket);

        // Act
        var result = await _ticketService.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Id);
    }
    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsFailure()
    {
        // Arrange
        _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Ticket?)null);
        // Act
        var result = await _ticketService.GetByIdAsync(999);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Квиток не знайдено", result.Message);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var createDto = new CreateTicketDto { SessionId = 1, SeatNumber = "A1", Price = 100 };
        var ticket = new Ticket { Id = 1, SessionId = 1, SeatNumber = "A1", Price = 100 };
        _ticketRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);
        _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ticket);
        // Act
        var result = await _ticketService.CreateAsync(createDto);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Data);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidData_ReturnsFailure()
    {
        int ticketId = 1;
        var dto = new UpdateTicketDto
        {
            SeatNumber = "1",
            Price = 100
        };
        _ticketRepositoryMock.Setup(l => l.GetByIdAsync(1)).ReturnsAsync((Ticket?)null);
        var result = await _ticketService.UpdateAsync(ticketId, dto);
        Assert.False(result.IsSuccess);
        Assert.Equal("Квиток не знайдено", result.Message);
    }

    [Fact]
    public async Task DeleteAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        int ticketId = 1;
        var ticket = new Ticket { Id = ticketId, SessionId = 1, SeatNumber = "A1", Price = 100 };
        _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(ticketId)).ReturnsAsync(ticket);
        _ticketRepositoryMock.Setup(repo => repo.DeleteAsync(ticket)).Returns(Task.CompletedTask);
        // Act
        var result = await _ticketService.DeleteAsync(ticketId);
        // Assert
        Assert.True(result.IsSuccess);
    }


    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFailure()
    {
        // Arrange
        int ticketId = 999;
        _ticketRepositoryMock.Setup(repo => repo.GetByIdAsync(ticketId)).ReturnsAsync((Ticket?)null);
        // Act
        var result = await _ticketService.DeleteAsync(ticketId);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Квиток не знайдено", result.Message);
    }




}
