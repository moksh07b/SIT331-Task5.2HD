using Gallery.Interfaces;
using Gallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gallery.Persistence
{
    public class ArtifactEF : IArtifactRepository
    {
        private readonly GalleryContext _context;

        public ArtifactEF(GalleryContext context)
        {
            _context = context;
        }

        // Get all artifacts
        public async Task<IEnumerable<Artifact>> GetAllAsync()
        {
            return await _context.Artifacts
                .Include(a => a.Artist) 
                .Include(a => a.Comments)
                .Include(a => a.Exhibition) 
                .ToListAsync();
        }

        // Get artifact by ID
        public async Task<Artifact?> GetByIdAsync(int id)
        {
            return await _context.Artifacts
                .Include(a => a.Artist) 
                .Include(a => a.Comments)
                .Include(a => a.Exhibition) 
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Add a new artifact
        public async Task AddAsync(Artifact artifact)
        {
            await _context.Artifacts.AddAsync(artifact);
            await _context.SaveChangesAsync();
        }

        // Update an existing artifact
        public async Task UpdateAsync(Artifact artifact)
        {
            _context.Artifacts.Update(artifact);
            await _context.SaveChangesAsync();
        }

        // Delete artifact by ID
        public async Task DeleteAsync(int id)
        {
            var artifact = await _context.Artifacts.FindAsync(id);
            if (artifact != null)
            {
                _context.Artifacts.Remove(artifact);
                await _context.SaveChangesAsync();
            }
        }

        // Check if an artifact exists by ID
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Artifacts.AnyAsync(a => a.Id == id);
        }
    }
}
