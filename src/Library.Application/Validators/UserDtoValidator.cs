using FluentValidation;
using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Validators
{
    public class UserDtoValidator : AbstractValidator<UserRequestDto>
    {
        public UserDtoValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(user => user.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == "Admin" || role == "Client").WithMessage("Role must be Admin or Client.");
        }
    }
}
