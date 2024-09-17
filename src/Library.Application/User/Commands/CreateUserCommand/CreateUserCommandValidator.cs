using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.User.Commands.CreateUserCommand
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserRequest.Email).NotEmpty();
            RuleFor(x => x.UserRequest.Password).NotEmpty();
            RuleFor(x => x.UserRequest.Role).NotEmpty();

        }
    }
}
