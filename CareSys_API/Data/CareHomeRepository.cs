using CareSys_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Data
{
    public class CareHomeRepository : ICareHomeRepository
    {
        private CareSysContext _context;

        public CareHomeRepository(CareSysContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CareHome>> GetAllCareHomesAsync()
        {
            return await _context.CareHomes
                .ToListAsync();
        }

        public async Task<IEnumerable<CareHome>> GetCareHomesByCategoryAsync(string categoryForSearch)
        {
            return await _context.CareHomes
                .Where(t => t.Category == categoryForSearch)
                .ToListAsync();
        }

        public async Task<IEnumerable<CareHome>> GetCareHomesByStatusAsync(Boolean isActive)
        {
            return await _context.CareHomes
                .Where(t => t.IsActive == isActive)
                .ToListAsync();
        }

        //public async Task<IEnumerable<CareHome>> Task<CareHome> Add(CareHome newCareHome);(Boolean newCareHome)
        //{
        //    return await _context.CareHomes.AddAsync()
        //}

        public async Task<CareHome> AddAsync(CareHome newCareHome)
        {
            newCareHome.IsActive = true;
            newCareHome.CreatedOn = DateTime.UtcNow;
            var careHome = await _context.CareHomes.AddAsync(newCareHome);
            await _context.SaveChangesAsync();

            return newCareHome;
        }
    }
}
