using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.CreateBookCommand
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookResponseDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Books.GetByISBNAsync(request.BookRequest.ISBN, cancellationToken);
            if (existingBook != null)
            {
                throw new InvalidOperationException("Book with such ISBN already exists.");
            }

            var book = _mapper.Map<Library.Domain.Models.Book>(request.BookRequest);
            await _unitOfWork.Books.AddAsync(book, cancellationToken);

            return _mapper.Map<BookResponseDto>(book);
        }
    }
}
