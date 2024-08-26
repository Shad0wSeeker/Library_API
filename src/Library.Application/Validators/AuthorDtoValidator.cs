using FluentValidation;
using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Validators
{
    public class AuthorDtoValidator : AbstractValidator<AuthorDto>
    {
        public AuthorDtoValidator()
        {
            RuleFor(author => author.AuthorFullName)
                .NotEmpty().WithMessage("Author's full name is required.")
                .MaximumLength(100).WithMessage("Author's full name must not exceed 100 characters.");

            RuleFor(author => author.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future.");

            RuleFor(author => author.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");
        }
    }
}
