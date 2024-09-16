using Library.Application.DTOs;
using Library.Shared.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Queries.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<PaginatedResultDto<AuthorResponseDto>>
    {
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetAllAuthorsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
