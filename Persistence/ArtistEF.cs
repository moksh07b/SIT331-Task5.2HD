using Gallery.Interfaces;
using Gallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gallery.Persistence
{
    public class ArtistEF : IArtistRepository
    {
        private readonly GalleryContext _context;

        public ArtistEF(GalleryContext context)
        {
            _context = context;
        }

        // Get all artists
        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _context.Artists
            .Include(a => a.Artifacts)
            .ToListAsync();
        }

        // Get artist by ID
        public async Task<Artist?> GetByIdAsync(int id)
        {
            return await _context.Artists
            .Include(a => a.Artifacts)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Add a new artist
        public async Task AddAsync(Artist artist)
        {
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        // Update an existing artist
        public async Task UpdateAsync(Artist artist)
        {
            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
        }

        // Delete artist by ID
        public async Task DeleteAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
        }

        // Check if an artist exists by ID
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Artists.AnyAsync(a => a.Id == id);
        }
    }
}
