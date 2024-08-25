using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Infrastructure.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users.Include(u=>u.BorrowedBooks).FirstOrDefaultAsync(u=>u.Id == userId);
        }
        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return user;
        }
       

        public async Task DeleteAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if(user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<User> GetByEmailAndPasswordAsync(string email, string password) 
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }


    }
}
