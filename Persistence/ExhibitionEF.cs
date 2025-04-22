using Gallery.Interfaces;
using Gallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gallery.Persistence
{
    public class ExhibitionEF : IExhibitionRepository
    {
        private readonly GalleryContext _context;

        public ExhibitionEF(GalleryContext context)
        {
            _context = context;
        }

        // Get all exhibitions
        public async Task<IEnumerable<Exhibition>> GetAllAsync()
        {
            return await _context.Exhibitions
            .Include(a => a.Artifacts)
            .ToListAsync();
        }

        // Get exhibition by ID
        public async Task<Exhibition?> GetByIdAsync(int id)
        {
            return await _context.Exhibitions
            .Include(a => a.Artifacts)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Add a new exhibition
        public async Task AddAsync(Exhibition exhibition)
        {
            await _context.Exhibitions.AddAsync(exhibition);
            await _context.SaveChangesAsync();
        }

        // Update an existing exhibition
        public async Task UpdateAsync(Exhibition exhibition)
        {
            _context.Exhibitions.Update(exhibition);
            await _context.SaveChangesAsync();
        }

        // Delete exhibition by ID
        public async Task DeleteAsync(int id)
        {
            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition != null)
            {
                _context.Exhibitions.Remove(exhibition);
                await _context.SaveChangesAsync();
            }
        }

        // Check if an exhibition exists by ID
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Exhibitions.AnyAsync(e => e.Id == id);
        }
    }
}
