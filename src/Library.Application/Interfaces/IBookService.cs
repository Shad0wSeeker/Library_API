using Library.Application.DTOs;
using Library.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<PaginatedResultDto<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize);
        Task<BookDto> GetBookByIdAsync(int id);
        Task<BookDto> GetBookByISBNAsync(string isbn);
        Task<BookDto> CreateBookAsync(BookDto BookDto);
        Task<BookDto> UpdateBookAsync(int id, BookDto bookDto);
        Task DeleteBookAsync(int id);
        Task<BorrowBookDto> BorrowBookAsync(BorrowBookDto borrowBookDto);
    }
}
