using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Commands.UpdateAuthorCommand
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Id must not be empty.");

            RuleFor(command => command.AuthorRequest)
                .NotNull().WithMessage("AuthorRequest must not be null.");

            RuleFor(command => command.AuthorRequest.AuthorFullName)
                .NotEmpty().WithMessage("AuthorFullName must not be empty.")
                .Matches(@"^\S+\s\S+$").WithMessage("AuthorFullName must consist of at least two words.");

            RuleFor(command => command.AuthorRequest.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("DateOfBirth must be in the past.");

            RuleFor(command => command.AuthorRequest.Country)
                .NotEmpty().WithMessage("Country must not be empty.");
        }
    }
}
