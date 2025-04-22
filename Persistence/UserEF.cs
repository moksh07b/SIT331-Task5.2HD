﻿using Gallery.Interfaces;
using Gallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gallery.Persistence
{
    public class UserEF : IUserRepository
    {
        private readonly GalleryContext _context;

        public UserEF(GalleryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Comments)
                .ToListAsync();
        }


        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {

            var password = user.PasswordHash;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {

            var password = user.PasswordHash;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<User>? GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
