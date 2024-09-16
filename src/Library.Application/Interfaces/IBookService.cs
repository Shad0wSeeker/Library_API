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
        Task<PaginatedResultDto<BookResponseDto>> GetAllBooksAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<BookResponseDto> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<BookResponseDto> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default);
        Task<BookResponseDto> CreateBookAsync(BookRequestDto BookDto, CancellationToken cancellationToken = default);
        Task<BookResponseDto> UpdateBookAsync(int id, BookRequestDto bookDto, CancellationToken cancellationToken = default);
        Task DeleteBookAsync(int id, CancellationToken cancellationToken = default);
        Task<BorrowBookDto> BorrowBookAsync(BorrowBookDto borrowBookDto, CancellationToken cancellationToken = default);
    }
}
