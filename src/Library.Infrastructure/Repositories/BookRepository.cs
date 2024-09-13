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
using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {

        public BookRepository(AppDbContext context) : base(context) { }

        public async Task<PaginatedResultDto<Book>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await GetAllAsync(pageNumber, pageSize, query => query.Include(b => b.Author), cancellationToken);
        }

        public async Task<Book> GetByISBNAsync(string ISBN, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(b => b.Author).FirstOrDefaultAsync(b => b.ISBN == ISBN, cancellationToken);
        }

    }
}
