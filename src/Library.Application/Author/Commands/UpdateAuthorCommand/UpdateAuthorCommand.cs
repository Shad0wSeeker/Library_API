using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Commands.UpdateAuthorCommand
{
    public class UpdateAuthorCommand : IRequest<AuthorResponseDto>
    {
        public int Id { get; }
        public AuthorRequestDto AuthorRequest { get; }

        public UpdateAuthorCommand(int id, AuthorRequestDto authorRequest)
        {
            Id = id;
            AuthorRequest = authorRequest;
        }
    }
}
