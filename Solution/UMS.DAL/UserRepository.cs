using UMS.Models;
using UMS.DAL.Shared;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace UMS.DAL
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return await _dbSet.AllAsync(u => u.Username != username);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _dbSet.AllAsync(u => u.Email != email);
        }
    }
}
