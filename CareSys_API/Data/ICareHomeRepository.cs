using CareSys_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Data
{
    public interface ICareHomeRepository
    {
        Task<IEnumerable<CareHome>> GetAllCareHomesAsync();

        Task<IEnumerable<CareHome>> GetCareHomesByCategoryAsync(string categoryForSearch);

        Task<IEnumerable<CareHome>> GetCareHomesByStatusAsync(Boolean isActive);

        // void Add<T>(T entity) where T : class;
        Task<CareHome> AddAsync(CareHome newCareHome);
    }
}
