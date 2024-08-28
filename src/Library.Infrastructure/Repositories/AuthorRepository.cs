using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Infrastructure.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Shared.DTO;

namespace Library.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResultDto<Author>> GetAllAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Authors.CountAsync();
            var items = await _context.Authors
                .Include(a => a.Books)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResultDto<Author>(items, totalCount, pageSize, pageNumber);
        }
        public async Task<Author> GetByIdAsync(int authorId)
        {
            return await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == authorId);
        }
        public async Task<Author> AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync(); 
            return author; 
        }
        public async Task<Author> UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync(); 
            return author;
        }

        public async Task DeleteAsync(int authorId)
        {
            var author = await GetByIdAsync(authorId);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync(); 
            }
        }
               
    }
}
