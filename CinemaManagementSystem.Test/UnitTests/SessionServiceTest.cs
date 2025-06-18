using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure.Services;
using Contracts.DTOs.SessionDTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CinemaManagementSystem.Test.UnitTests;

public class SessionServiceTest
{
    private readonly Mock<ISessionRepository> _sessionRpositoryMock;
    private readonly SessionService _sessionService;
    public SessionServiceTest()
    {
        _sessionRpositoryMock = new Mock<ISessionRepository>();
        _sessionService = new SessionService(_sessionRpositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSessions()
    {
        // Arrange
        var sessions = new List<Session>
        {
            new Session { Id = 1, FilmId = 1, Time = DateTime.Now, HallId = 1 },
            new Session { Id = 2, FilmId = 2, Time = DateTime.Now.AddHours(1), HallId = 2 }
        };
        _sessionRpositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sessions);
        // Act
        var result = await _sessionService.GetAllAsync();
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetAllAsync_WithEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        _sessionRpositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Session>());
        // Act
        var result = await _sessionService.GetAllAsync();
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSession_WhenExists()
    {
        // Arrange
        var session = new Session { Id = 1, FilmId = 1, Time = DateTime.Now, HallId = 1 };
        _sessionRpositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(session);
        // Act
        var result = await _sessionService.GetByIdAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(session.Id, result.Data.Id);
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenSessionDoesNotExist()
    {
        // Arrange
        _sessionRpositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Session?)null);
        // Act
        var result = await _sessionService.GetByIdAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Сеанс не знайдено", result.Message);
    }
    [Fact]
    public async Task CreateAsync_ShouldCreateSession()
    {
        // Arrange
        var createDto = new CreateSessionDto { FilmId = 1, StartTime = DateTime.Now, HallId = 1 };
        _sessionRpositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Session>())).ReturnsAsync(1);
        // Act
        var result = await _sessionService.CreateAsync(createDto);
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data);
    }
    [Fact]
    public async Task UpdateAsync_ShouldUpdateSession_WhenExists()
    {
        // Arrange
        var updateDto = new UpdateSessionDto { FilmId = 1, StartTime = DateTime.Now, HallId = 1 };
        var existingSession = new Session { Id = 1, FilmId = 1, Time = DateTime.Now.AddHours(-1), HallId = 1 };
        _sessionRpositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingSession);
        _sessionRpositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Session>())).ReturnsAsync(true);
        // Act
        var result = await _sessionService.UpdateAsync(1, updateDto);
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
    [Fact]
    public async Task UpdateAsync_ShouldReturnFailure_WhenSessionDoesNotExist()
    {
        // Arrange
        var updateDto = new UpdateSessionDto { FilmId = 1, StartTime = DateTime.Now, HallId = 1 };
        _sessionRpositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Session?)null);
        // Act
        var result = await _sessionService.UpdateAsync(1, updateDto);
        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Сеанс не знайдено", result.Message);
    }
    [Fact]
    public async Task DeleteAsync_ShouldDeleteSession_WhenExists()
    {
        // Arrange
        var existingSession = new Session { Id = 1, FilmId = 1, Time = DateTime.Now, HallId = 1 };
        _sessionRpositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingSession);
        _sessionRpositoryMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);
        // Act
        var result = await _sessionService.DeleteAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenSessionDoesNotExist()
    {
        // Arrange
        _sessionRpositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Session?)null);
        // Act
        var result = await _sessionService.DeleteAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Сеанс не знайдено", result.Message);
    }
    [Fact]
    public async Task GetByFilmIdAsync_ShouldReturnSessions_WhenExists()
    {
        // Arrange
        var sessions = new List<Session>
        {
            new Session { Id = 1, FilmId = 1, Time = DateTime.Now, HallId = 1 },
            new Session { Id = 2, FilmId = 1, Time = DateTime.Now.AddHours(1), HallId = 2 }
        };
        _sessionRpositoryMock.Setup(repo => repo.GetByFilmIdAsync(1)).ReturnsAsync(sessions);
        // Act
        var result = await _sessionService.GetByFilmIdAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetByFilmIdAsync_ShouldReturnFailure_WhenNoSessionsExist()
    {
        // Arrange
        _sessionRpositoryMock
            .Setup(repo => repo.GetByFilmIdAsync(1))
            .ReturnsAsync(new List<Session>());

        // Act
        var result = await _sessionService.GetByFilmIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data); // <- ✅ правильна перевірка!
        Assert.Equal("Сеанси не знайдено", result.Message);
    }

    [Fact]
    public async Task GetByFilmIdAsync_ShouldReturnFailure_WhenFilmDoesNotExist()
    {
        // Arrange
        _sessionRpositoryMock.Setup(repo => repo.GetByFilmIdAsync(1)).ReturnsAsync((List<Session>?)null);
        // Act
        var result = await _sessionService.GetByFilmIdAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Сеанси не знайдено", result.Message);
    }
    
    [Fact]
    public async Task GetByFilmIdAsync_ShouldReturnSessions_WhenFilmHasSessions()
    {
        // Arrange
        var sessions = new List<Session>
        {
            new Session { Id = 1, FilmId = 1, Time = DateTime.Now, HallId = 1 },
            new Session { Id = 2, FilmId = 1, Time = DateTime.Now.AddHours(1), HallId = 2 }
        };
        _sessionRpositoryMock.Setup(repo => repo.GetByFilmIdAsync(1)).ReturnsAsync(sessions);
        // Act
        var result = await _sessionService.GetByFilmIdAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
    }
    

}
