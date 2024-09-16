using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.CreateBookCommand
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.BookRequest.ISBN).NotEmpty().WithMessage("ISBN is required.");
            RuleFor(x => x.BookRequest.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.BookRequest.Genre).NotEmpty().WithMessage("Genre is required.");
            RuleFor(x => x.BookRequest.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.BookRequest.AuthorId).NotEmpty().WithMessage("AuthorId is required.");
        }
    }
}
