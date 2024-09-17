using FluentValidation;
using Library.Application.DTOs;


namespace Library.Application.Author.Commands.CreateAuthorCommand
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(command => command.AuthorRequest).NotNull();
            RuleFor(command => command.AuthorRequest.AuthorFullName).NotEmpty();
            RuleFor(command => command.AuthorRequest.DateOfBirth).NotEmpty();
            RuleFor(command => command.AuthorRequest.Country).NotEmpty();
        }
    }
}
