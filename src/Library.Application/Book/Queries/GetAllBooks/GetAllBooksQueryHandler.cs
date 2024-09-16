using AutoMapper;
using Library.Application.Author.Queries.GetAllAuthors;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using Library.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PaginatedResultDto<BookResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBooksQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResultDto<BookResponseDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var paginatedBooks = await _unitOfWork.Books.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                query => query.Include(b => b.Author),
                cancellationToken
            );

            var bookDtos = _mapper.Map<IEnumerable<BookResponseDto>>(paginatedBooks.Items);

            return new PaginatedResultDto<BookResponseDto>(bookDtos, paginatedBooks.TotalCount, request.PageSize, request.PageNumber);
        }
    }
}
