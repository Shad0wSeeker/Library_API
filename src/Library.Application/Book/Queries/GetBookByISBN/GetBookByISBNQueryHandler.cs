using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Queries.GetBookByISBN
{
    public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, BookResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetBookByISBNQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookResponseDto> Handle(GetBookByISBNQuery request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByISBNAsync(request.ISBN, cancellationToken);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }
            return _mapper.Map<BookResponseDto>(book);
        }
    }
}
