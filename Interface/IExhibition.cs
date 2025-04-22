using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Models;

namespace Gallery.Interfaces
{
    public interface IExhibitionRepository
    {
        Task<IEnumerable<Exhibition>> GetAllAsync();
        Task<Exhibition?> GetByIdAsync(int id);
        Task AddAsync(Exhibition exhibition);
        Task UpdateAsync(Exhibition exhibition);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
