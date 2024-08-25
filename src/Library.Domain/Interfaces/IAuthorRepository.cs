using Library.Domain.Models;
using Library.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<PaginatedResultDto<Author>> GetAllAsync(int pageNumber, int pageSize);
        Task<Author> GetByIdAsync(int authorId);
        Task<Author> AddAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task DeleteAsync(int authorId);
    }
}
