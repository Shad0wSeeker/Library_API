using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.BorrowBookCommand
{
    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, BorrowBookDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BorrowBookDto> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(request.BookId, cancellationToken);
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

            if (book == null || user == null)
            {
                throw new InvalidOperationException("Book or user not found.");
            }

            if (book.BorrowingTime != default && book.ReturningTime > DateTime.Now)
            {
                throw new InvalidOperationException("The book is already borrowed.");
            }

            if (request.ReturningTime < request.BorrowingTime)
            {
                throw new ArgumentException("The returning time must be greater than the borrowing time.");
            }

            book.BorrowingTime = request.BorrowingTime;
            book.ReturningTime = request.ReturningTime;

            user.BorrowedBooks ??= new List<Library.Domain.Models.Book>();
            user.BorrowedBooks.Add(book);

            await _unitOfWork.Books.UpdateAsync(book, cancellationToken);
            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);

            var borrowedBookDto = new BorrowBookDto
            {
                BookId = book.Id,
                UserId = user.Id,
                BorrowingTime = request.BorrowingTime,
                ReturningTime = request.ReturningTime
            };

            return borrowedBookDto;
        }
    }
}
