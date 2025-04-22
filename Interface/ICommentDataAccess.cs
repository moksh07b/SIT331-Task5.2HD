using System.Collections.Generic;
using System.Threading.Tasks;
using Gallery.Models;

namespace Gallery.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
        Task<IEnumerable<Comment>> GetByArtifactIdAsync(int artifactId);
    }
}
