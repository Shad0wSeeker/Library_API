using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.UpdateBookCommand
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBookCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookResponseDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.Id, cancellationToken);

            if (book == null)
            {
                throw new KeyNotFoundException("Book not found.");
            }

            _mapper.Map(request.BookRequest, book);
            await _unitOfWork.Books.UpdateAsync(book, cancellationToken);

            return _mapper.Map<BookResponseDto>(book);
        }
    }
}
