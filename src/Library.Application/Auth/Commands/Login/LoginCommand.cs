using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Auth.Commands.Login
{
    public class LoginCommand : IRequest<string> 
    {
        public UserResponseDto User { get; set; }

        public LoginCommand(UserResponseDto user)
        {
            User = user;
        }
    }
}
