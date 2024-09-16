using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.User.Queries.AuthenticateUser
{
    public class AuthenticateUserQuery : IRequest<UserResponseDto>
    {
        public string Email { get; }

        public AuthenticateUserQuery(string email)
        {
            Email = email;
        }
    }
}
