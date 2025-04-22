
using Gallery.Models;

namespace Gallery.Interfaces
{
    public interface IArtifactRepository
    {
        Task<IEnumerable<Artifact>> GetAllAsync();
        Task<Artifact?> GetByIdAsync(int id);
        Task AddAsync(Artifact artifact);
        Task UpdateAsync(Artifact artifact);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
