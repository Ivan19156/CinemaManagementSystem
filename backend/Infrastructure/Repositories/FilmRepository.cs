using Application.Interfaces.Repositories;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly CinemaDbContext _context;
        public FilmRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Film>> GetAllAsync()
        {
            return await _context.Films.ToListAsync();
        }
        public async Task<Film?> GetByIdAsync(int id)
        {
            return await _context.Films.FindAsync(id);
        }
        public async Task<int> CreateAsync(Film film)
        {
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            return film.Id;
        }
        public async Task<bool> UpdateAsync(Film film)
        {
            _context.Films.Update(film);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var film = await GetByIdAsync(id);
            if (film == null) return false;
            _context.Films.Remove(film);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
