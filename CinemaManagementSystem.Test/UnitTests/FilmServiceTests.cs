using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using CinemaManagementSystem.Infrastructure.Services;
using Contracts.DTOs.FilmDTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CinemaManagementSystem.Test.UnitTests;

public class FilmServiceTests
{
    private readonly Mock<IFilmRepository> _filmRepositoryMock;
    private readonly FilmService _filmService;

    public FilmServiceTests()
    {
        _filmRepositoryMock = new Mock<IFilmRepository>();
        _filmService = new FilmService(_filmRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllFilmsAsync_ReturnsListOfFilmDtos()
    {
        var films = new List<Film> { new Film { Id = 1, Name = "Test", Jenre = "Action", Director = "John", ReleaseDate = DateTime.Today, Description = "Desc" } };
        _filmRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(films);

        var result = await _filmService.GetAllFilmsAsync();

        Assert.True(result.IsSuccess);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task GetFilmByIdAsync_WhenFilmExists_ReturnsDto()
    {
        var film = new Film { Id = 1, Name = "Test", Jenre = "Action", Director = "John", ReleaseDate = DateTime.Today, Description = "Desc" };
        _filmRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(film);

        var result = await _filmService.GetFilmByIdAsync(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data.Id);
    }

    [Fact]
    public async Task GetFilmByIdAsync_WhenFilmNotFound_ReturnsFailure()
    {
        _filmRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Film?)null);

        var result = await _filmService.GetFilmByIdAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("Фільм не знайдено", result.Message);
    }

    [Fact]
    public async Task AddFilmAsync_CreatesFilm_ReturnsId()
    {
        var dto = new CreateFilmDto { Title = "Test", Genre = "Action", Director = "John", ReleaseDate = DateTime.Today, Description = "Desc" };
        _filmRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Film>())).ReturnsAsync(10);

        var result = await _filmService.AddFilmAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Data);
    }

    [Fact]
    public async Task UpdateFilmAsync_WhenFilmExists_UpdatesAndReturnsSuccess()
    {
        var film = new Film { Id = 1 };
        var dto = new UpdateFilmDto { Id = 1, Title = "New", Genre = "Drama", Director = "Jane", ReleaseDate = DateTime.Today, Description = "Updated" };
        _filmRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(film);
        _filmRepositoryMock.Setup(r => r.UpdateAsync(film)).ReturnsAsync(true);

        var result = await _filmService.UpdateFilmAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
    }

    [Fact]
    public async Task UpdateFilmAsync_WhenFilmNotFound_ReturnsFailure()
    {
        var dto = new UpdateFilmDto { Id = 1 };
        _filmRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Film?)null);

        var result = await _filmService.UpdateFilmAsync(dto);

        Assert.False(result.IsSuccess);
        Assert.Equal("Фільм не знайдено", result.Message);
    }

    [Fact]
    public async Task DeleteFilmAsync_WhenFilmExists_ReturnsSuccess()
    {
        _filmRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _filmService.DeleteFilmAsync(1);

        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
    }

    [Fact]
    public async Task DeleteFilmAsync_WhenDeleteFails_ReturnsFailure()
    {
        _filmRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _filmService.DeleteFilmAsync(1);

        Assert.False(result.IsSuccess);
        Assert.Equal("Фільм не знайдено або не вдалося видалити", result.Message);
    }

    [Fact]
    public async Task SearchFilmsAsync_ReturnsMatchingFilms()
    {
        var films = new List<Film>
        {
            new Film { Name = "Avangers" },
            new Film { Name = "Avatar" },
            new Film { Name = "Inception" }
        };
        _filmRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(films);

        var result = await _filmService.SearchFilmsAsync("ava");

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetFilmsByGenreAsync_ReturnsFilteredByGenre()
    {
        var films = new List<Film>
        {
            new Film { Jenre = "Action" },
            new Film { Jenre = "Drama" },
            new Film { Jenre = "Action" }
        };
        _filmRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(films);

        var result = await _filmService.GetFilmsByGenreAsync("Action");

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetFilmsByDirectorAsync_ReturnsFilteredByDirector()
    {
        var films = new List<Film>
        {
            new Film { Director = "Nolan" },
            new Film { Director = "Nolan" },
            new Film { Director = "Tarantino" }
        };
        _filmRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(films);

        var result = await _filmService.GetFilmsByDirectorAsync("Nolan");

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetFilmsByReleaseDateAsync_ReturnsFilteredByDate()
    {
        var date = new DateTime(2023, 1, 1);
        var films = new List<Film>
        {
            new Film { ReleaseDate = date },
            new Film { ReleaseDate = DateTime.Today }
        };
        _filmRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(films);

        var result = await _filmService.GetFilmsByReleaseDateAsync(date);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Data);
    }
}
