using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.UpdateBookCommand
{
    public class UpdateBookCommand : IRequest<BookResponseDto>
    {
        public int Id { get; }
        public BookRequestDto BookRequest { get; set; }

        public UpdateBookCommand(int id, BookRequestDto bookRequest)
        {
            Id = id;
            BookRequest = bookRequest;

        }
    }
}
