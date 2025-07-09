using MongoDB.Driver;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Ticket> _collection;

    public TicketRepository(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoDb:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
        _collection = database.GetCollection<Ticket>("Tickets");

        // Індекс для унікальності TicketId
        var indexKeys = Builders<Ticket>.IndexKeys.Ascending(t => t.TicketId);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var model = new CreateIndexModel<Ticket>(indexKeys, indexOptions);
        _collection.Indexes.CreateOne(model);
    }


    public async Task AddAsync(Ticket ticket)
    {
        await _collection.InsertOneAsync(ticket);
    }

    public async Task<bool> ExistsAsync(int ticketId)
    {
        var filter = Builders<Ticket>.Filter.Eq(t => t.TicketId, ticketId);
        return await _collection.Find(filter).AnyAsync();
    }
}

