using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.Include(a=>a.Books).ToListAsync();
        }
        public async Task<Author> GetByIdAsync(int authorId)
        {
            return await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == authorId);
        }
        public async Task<Author> AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            return author;
            
        }
        public async Task<Author> UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            return author;
        }

        public async Task DeleteAsync(int authorId)
        {
            var author = await GetByIdAsync(authorId);
            if(author != null)
            {
                _context.Authors.Remove(author);
            }
        }

       

        

        
    }
}
