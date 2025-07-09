using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using CinemaManagementSystem.WebAPI.Helpers;
using Contracts.DTOs.FilmDTOs;
using Shared.Helpers;
namespace Application.Interfaces.Services
{
    public interface IFilmService
    {
        Task<ServiceResult<IEnumerable<FilmDto>>> GetAllFilmsAsync();
        Task<ServiceResult<FilmDto>> GetFilmByIdAsync(int id);
        Task<ServiceResult<int>> AddFilmAsync(CreateFilmDto createDto);
        Task<ServiceResult<bool>> UpdateFilmAsync(UpdateFilmDto updateDto);
        Task<ServiceResult<bool>> DeleteFilmAsync(int id);
        Task<ServiceResult<IEnumerable<FilmDto>>> SearchFilmsAsync(string searchTerm);
        Task<ServiceResult<IEnumerable<FilmDto>>> GetFilmsByGenreAsync(string genre);
        Task<ServiceResult<IEnumerable<FilmDto>>> GetFilmsByDirectorAsync(string director);
        Task<ServiceResult<IEnumerable<FilmDto>>> GetFilmsByReleaseDateAsync(DateTime releaseDate);
        
    }

}
