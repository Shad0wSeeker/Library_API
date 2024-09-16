using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Commands.CreateAuthorCommand
{
    public class CreateAuthorCommand : IRequest<AuthorResponseDto>
    {
        public AuthorRequestDto AuthorRequest { get; }

        public CreateAuthorCommand(AuthorRequestDto authorRequest)
        {
            AuthorRequest = authorRequest;
        }
    }
}
