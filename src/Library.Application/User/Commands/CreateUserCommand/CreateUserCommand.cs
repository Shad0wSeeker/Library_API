using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.User.Commands.CreateUserCommand
{
    public class CreateUserCommand : IRequest<UserResponseDto>
    {
        public UserRequestDto UserRequest { get; }

        public CreateUserCommand(UserRequestDto userRequest)
        {
            UserRequest = userRequest;
        }
    }
}
