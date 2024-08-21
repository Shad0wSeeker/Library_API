﻿using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.Include(b=>b.Author).ToListAsync();
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
            await _context.Books.AddAsync(book);
            return book;
        }
        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            return book;
        }

        public async Task DeleteAsync(int bookId)
        {
            var book = await GetByIdAsync(bookId);
            if(book != null)
            {
                _context.Books.Remove(book);
            }
        }
                        
    }
}
