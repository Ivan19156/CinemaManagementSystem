using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly CinemaDbContext _context;

        public SessionRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Session>> GetAllAsync()
        {
            return await _context.Sessions
                .Include(s => s.Film)
                .Include(s => s.Hall)
                .ToListAsync();
        }

        public async Task<Session?> GetByIdAsync(int id)
        {
            return await _context.Sessions
                .Include(s => s.Film)
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> CreateAsync(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session.Id;
        }

        public async Task<bool> UpdateAsync(Session session)
        {
            _context.Sessions.Update(session);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = await GetByIdAsync(id);
            if (session == null)
                return false;

            _context.Sessions.Remove(session);
            var affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<List<Session>> GetByFilmIdAsync(int filmId)
        {
            return await _context.Sessions
                .Include(s => s.Film)
                .Include(s => s.Hall)
                .Where(s => s.FilmId == filmId)
                .ToListAsync();
        }

    }

}
