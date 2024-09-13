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
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext context) : base(context) { }

        public async Task<PaginatedResultDto<Author>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await GetAllAsync(pageNumber, pageSize, query => query.Include(a => a.Books));
        }
                             
    }
}
