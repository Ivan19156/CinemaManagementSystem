using CinemaManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFilmRepository
    {
        Task<List<Film>> GetAllAsync();
        Task<Film?> GetByIdAsync(int id);
        Task<int> CreateAsync(Film film);
        Task<bool> UpdateAsync(Film film);
        Task<bool> DeleteAsync(int id);
        

    }
}
