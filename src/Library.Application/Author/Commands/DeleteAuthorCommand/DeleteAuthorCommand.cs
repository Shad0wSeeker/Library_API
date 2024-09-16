using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Commands.DeleteAuthorCommand
{
    public class DeleteAuthorCommand : IRequest<Unit>
    {
        public int Id { get; }

        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }
    }
}
