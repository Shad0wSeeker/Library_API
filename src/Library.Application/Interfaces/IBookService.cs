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
        Task<PaginatedResultDto<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<BookDto> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<BookDto> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task<BookDto> CreateBookAsync(BookDto BookDto, CancellationToken cancellationToken = default);
        Task<BookDto> UpdateBookAsync(int id, BookDto bookDto, CancellationToken cancellationToken = default);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
        Task<BorrowBookDto> BorrowBookAsync(BorrowBookDto borrowBookDto, CancellationToken cancellationToken = default);
    }
}
