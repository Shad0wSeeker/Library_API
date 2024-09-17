using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.User.Queries.AuthenticateUser
{
    public class AuthenticateUserQueryValidator : AbstractValidator<AuthenticateUserQuery>
    {
        public AuthenticateUserQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
