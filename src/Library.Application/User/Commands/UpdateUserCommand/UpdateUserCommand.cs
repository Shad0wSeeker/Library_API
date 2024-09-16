using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.User.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest<UserResponseDto>
    {
        public int Id { get; }
        public UserRequestDto UserRequest { get; }

        public UpdateUserCommand(int id, UserRequestDto userRequest)
        {
            Id = id;
            UserRequest = userRequest;
        }
    }
}
