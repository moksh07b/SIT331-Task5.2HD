using Gallery.Interfaces;
using Gallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gallery.Persistence
{
    public class CommentEF : ICommentRepository
    {
        private readonly GalleryContext _context;

        public CommentEF(GalleryContext context)
        {
            _context = context;
        }

        // Get all comments
        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.Artifact)  
                .Include(c => c.User)      
                .ToListAsync();
        }

        // Get comment by ID
        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.Artifact)  
                .Include(c => c.User)      
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Add a new comment
        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        // Update an existing comment
        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        // Delete comment by ID
        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        // Get comments by Artifact ID
        public async Task<IEnumerable<Comment>> GetByArtifactIdAsync(int artifactId)
        {
            return await _context.Comments
                .Where(c => c.ArtifactId == artifactId)
                .Include(c => c.Artifact)  
                .Include(c => c.User)      
                .ToListAsync();
        }
    }
}
