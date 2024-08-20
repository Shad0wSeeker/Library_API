using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int authorId);
        Task<Author> AddAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task DeleteAsync(int authorId);
    }
}
