using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Queries.GetBookByISBN
{
    public class GetBookByISBNQuery : IRequest<BookResponseDto>
    {
        public string ISBN { get;  }

        public GetBookByISBNQuery(string isbn)
        {
            ISBN = isbn;
        }
    }

}
