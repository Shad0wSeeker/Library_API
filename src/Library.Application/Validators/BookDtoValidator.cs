using FluentValidation;
using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Validators
{
    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {
            RuleFor(book => book.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")                
                .MaximumLength(13).WithMessage("ISBN must be exactly 13 characters long.");

            RuleFor(book => book.Name)
                .NotEmpty().WithMessage("Book name is required.")
                .MaximumLength(150).WithMessage("Book name must not exceed 150 characters.");

            RuleFor(book => book.Genre)
                .NotEmpty().WithMessage("Genre is required.")
                .MaximumLength(50).WithMessage("Genre must not exceed 50 characters.");

            RuleFor(book => book.BorrowingTime)
                .LessThan(book => book.ReturningTime).WithMessage("Borrowing time must be before returning time.");
        }
    }
}
