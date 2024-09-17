﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.UpdateBookCommand
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.BookRequest).NotNull();
            RuleFor(x => x.BookRequest.ISBN).NotEmpty();
            RuleFor(x => x.BookRequest.Name).NotEmpty();
            RuleFor(x => x.BookRequest.Genre).NotEmpty();
            RuleFor(x => x.BookRequest.Description).NotEmpty();
            RuleFor(x => x.BookRequest.AuthorId).NotEmpty();
        }
    }
}
