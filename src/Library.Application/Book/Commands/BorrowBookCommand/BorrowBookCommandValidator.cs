using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.BorrowBookCommand
{
    public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
    {
        public BorrowBookCommandValidator()
        {
            RuleFor(x => x.BookId).GreaterThan(0).WithMessage("BookId must be greater than zero.");
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than zero.");
            RuleFor(x => x.BorrowingTime).LessThan(x => x.ReturningTime).WithMessage("Borrowing time must be before returning time.");
        }
    }
}
