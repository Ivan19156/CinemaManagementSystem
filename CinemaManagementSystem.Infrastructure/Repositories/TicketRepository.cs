using Application.Interfaces.Repositories;
using CinemaManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaDbContext _context;
        public TicketRepository(CinemaDbContext context)
        {
            _context = context;
        }
        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }
        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();

        }
        public async Task<Ticket?> GetBySessionId(int sessionId)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(t => t.SessionId == sessionId);
        }
        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
