using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Infrastructure.Data;
using Library.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResultDto<Book>> GetAllAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Books.CountAsync();
            var items = await _context.Books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(b => b.Author)
                .ToListAsync();

            return new PaginatedResultDto<Book>(items, totalCount, pageSize, pageNumber);
        }

        public async Task<Book> GetByIdAsync(int bookId)
        {
            return await _context.Books.Include(b=>b.Author).FirstOrDefaultAsync(b=>b.Id == bookId);
        }

        public async Task<Book> GetByISBNAsync(string ISBN)
        {
            return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.ISBN == ISBN);

        }
        public async Task<Book> AddAsync(Book book)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == book.ISBN);
            if (existingBook != null)
            {
                throw new InvalidOperationException("Book with this ISBN already exists.");
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }
        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteAsync(int bookId)
        {
            var book = await GetByIdAsync(bookId);
            if(book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
                        
    }
}
