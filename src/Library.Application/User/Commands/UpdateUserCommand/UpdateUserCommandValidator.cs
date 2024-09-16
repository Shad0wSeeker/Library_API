using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.User.Commands.UpdateUserCommand
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id must not be empty.");

            RuleFor(x => x.UserRequest.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.UserRequest.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.UserRequest.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Matches(@"^(Admin|Client)$").WithMessage("Role must be either 'Admin' or 'Client'.");
        }
    }
}
