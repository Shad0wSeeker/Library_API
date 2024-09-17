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
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.AuthorRequest).NotEmpty();
            RuleFor(command => command.AuthorRequest.AuthorFullName).NotEmpty();
            RuleFor(command => command.AuthorRequest.DateOfBirth).NotEmpty();
            RuleFor(command => command.AuthorRequest.Country).NotEmpty();
        }
    }
}
