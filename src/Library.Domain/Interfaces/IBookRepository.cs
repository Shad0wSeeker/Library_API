using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int bookId);
        Task<Book> GetByISBNAsync(string ISBN);
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task DeleteAsync(int bookId);


    }
}
