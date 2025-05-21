using CinemaManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        Task<List<Session>> GetAllAsync();
        Task<Session?> GetByIdAsync(int id);
        Task<int> CreateAsync(Session session);
        Task<bool> UpdateAsync(Session session);
        Task<bool> DeleteAsync(int id);
        Task<List<Session>> GetByFilmIdAsync(int filmId);

    }
}
