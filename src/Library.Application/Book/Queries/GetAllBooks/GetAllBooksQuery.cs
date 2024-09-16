using Library.Application.DTOs;
using Library.Shared.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<PaginatedResultDto<BookResponseDto>>
    {
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetAllBooksQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
