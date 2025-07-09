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
    public class HallRepository : IHallRepository
    {
        private readonly CinemaDbContext _context;
        public HallRepository(CinemaDbContext context)
        {
            _context = context;
        }
        public async Task<List<Hall>> GetAllAsync()
        {
            return await _context.Halls.ToListAsync();
        }
        public async Task<Hall?> GetByIdAsync(int id)
        {
            return await _context.Halls.FindAsync(id);
        }
        public async Task<int> CreateAsync(Hall hall)
        {
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();
            return hall.Id;
        }
        public async Task<bool> UpdateAsync(Hall hall)
        {
            _context.Halls.Update(hall);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var hall = await GetByIdAsync(id);
            if (hall == null) return false;
            _context.Halls.Remove(hall);
            return await _context.SaveChangesAsync() > 0;
        }

        
    }
}
