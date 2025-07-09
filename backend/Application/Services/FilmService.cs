using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.FilmDTOs;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;

    public FilmService(IFilmRepository filmRepository)
    {
        _filmRepository = filmRepository;
    }

    public async Task<ServiceResult<IEnumerable<FilmDto>>> GetAllFilmsAsync()
    {
        var films = await _filmRepository.GetAllAsync();
        var dtos = films.Select(MapToDto);
        return ServiceResult<IEnumerable<FilmDto>>.Success(dtos);
    }

    public async Task<ServiceResult<FilmDto>> GetFilmByIdAsync(int id)
    {
        var film = await _filmRepository.GetByIdAsync(id);
        if (film == null)
            return ServiceResult<FilmDto>.Failure("Фільм не знайдено");
        return ServiceResult<FilmDto>.Success(MapToDto(film));
    }

    public async Task<ServiceResult<int>> AddFilmAsync(CreateFilmDto createDto)
    {
        var film = new Film
        {
            Name = createDto.Title,
            Jenre = createDto.Genre,
            Director = createDto.Director,
            ReleaseDate = createDto.ReleaseDate,
            Description = createDto.Description
        };

        var id = await _filmRepository.CreateAsync(film);
        return ServiceResult<int>.Success(id);
    }

    public async Task<ServiceResult<bool>> UpdateFilmAsync(UpdateFilmDto updateDto)
    {
        var existing = await _filmRepository.GetByIdAsync(updateDto.Id);
        if (existing == null)
            return ServiceResult<bool>.Failure("Фільм не знайдено");

        existing.Name = updateDto.Title;
        existing.Jenre = updateDto.Genre;
        existing.Director = updateDto.Director;
        existing.ReleaseDate = updateDto.ReleaseDate;
        existing.Description = updateDto.Description;

        var success = await _filmRepository.UpdateAsync(existing);
        return ServiceResult<bool>.Success(success);
    }

    public async Task<ServiceResult<bool>> DeleteFilmAsync(int id)
    {
        var success = await _filmRepository.DeleteAsync(id);
        if (!success)
            return ServiceResult<bool>.Failure("Фільм не знайдено або не вдалося видалити");
        return ServiceResult<bool>.Success(true);
    }

    public async Task<ServiceResult<IEnumerable<FilmDto>>> SearchFilmsAsync(string searchTerm)
    {
        var all = await _filmRepository.GetAllAsync();
        var filtered = all
            .Where(f => f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .Select(MapToDto);
        return ServiceResult<IEnumerable<FilmDto>>.Success(filtered);
    }

    public async Task<ServiceResult<IEnumerable<FilmDto>>> GetFilmsByGenreAsync(string genre)
    {
        var all = await _filmRepository.GetAllAsync();
        var filtered = all
            .Where(f => f.Jenre.Equals(genre, StringComparison.OrdinalIgnoreCase))
            .Select(MapToDto);
        return ServiceResult<IEnumerable<FilmDto>>.Success(filtered);
    }

    public async Task<ServiceResult<IEnumerable<FilmDto>>> GetFilmsByDirectorAsync(string director)
    {
        var all = await _filmRepository.GetAllAsync();
        var filtered = all
            .Where(f => f.Director.Equals(director, StringComparison.OrdinalIgnoreCase))
            .Select(MapToDto);
        return ServiceResult<IEnumerable<FilmDto>>.Success(filtered);
    }

    public async Task<ServiceResult<IEnumerable<FilmDto>>> GetFilmsByReleaseDateAsync(DateTime releaseDate)
    {
        var all = await _filmRepository.GetAllAsync();
        var filtered = all
            .Where(f => f.ReleaseDate.Date == releaseDate.Date)
            .Select(MapToDto);
        return ServiceResult<IEnumerable<FilmDto>>.Success(filtered);
    }

    private FilmDto MapToDto(Film film)
    {
        return new FilmDto
        {
            Id = film.Id,
            Title = film.Name,
            Genre = film.Jenre,
            Director = film.Director,
            ReleaseDate = film.ReleaseDate,
            Description = film.Description
        };
    }

      
}
