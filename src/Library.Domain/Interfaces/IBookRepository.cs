using Library.Domain.Models;
using Library.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<PaginatedResultDto<Book>> GetAllAsync(int pageNumber, int pageSize);
        Task<Book> GetByIdAsync(int bookId);
        Task<Book> GetByISBNAsync(string ISBN);
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task DeleteAsync(int bookId);


    }
}
