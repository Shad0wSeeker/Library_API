using FluentValidation;
using Library.Application.DTOs;


namespace Library.Application.Author.Commands.CreateAuthorCommand
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(command => command.AuthorRequest)
                .NotNull()
                .WithMessage("AuthorRequest cannot be null");

            RuleFor(command => command.AuthorRequest.AuthorFullName)
                .NotEmpty()
                .WithMessage("AuthorFullName is required")
                .Matches(@"^[\p{L}]+(\s[\p{L}]+)+$")
                .WithMessage("AuthorFullName must consist of at least two words");

            RuleFor(command => command.AuthorRequest.DateOfBirth)
                .NotEmpty()
                .WithMessage("DateOfBirth is required")
                .LessThan(DateTime.Now)
                .WithMessage("DateOfBirth must be in the past");

            RuleFor(command => command.AuthorRequest.Country)
                .NotEmpty()
                .WithMessage("Country is required");
        }
    }
}
