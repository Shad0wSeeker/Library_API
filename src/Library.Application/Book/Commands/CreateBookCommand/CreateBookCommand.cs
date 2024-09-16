using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.CreateBookCommand
{
    public class CreateBookCommand : IRequest<BookResponseDto>
    {
        public BookRequestDto BookRequest { get; }

        public CreateBookCommand(BookRequestDto bookRequest)
        {
            BookRequest = bookRequest;
        }
    }
}
