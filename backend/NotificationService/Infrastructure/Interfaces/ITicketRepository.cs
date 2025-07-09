using NotificationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Interfaces;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket);
    Task<bool> ExistsAsync(int ticketId);
}
