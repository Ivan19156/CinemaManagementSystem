using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IHallRepository
    {
        Task<List<Hall>> GetAllAsync();
        Task<Hall?> GetByIdAsync(int id);
        Task<int> CreateAsync(Hall hall);
        Task<bool> UpdateAsync(Hall hall);
        Task<bool> DeleteAsync(int id);
    }
}
