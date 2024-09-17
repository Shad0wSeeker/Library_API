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
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.BorrowingTime).NotEmpty();
        }
    }
}
