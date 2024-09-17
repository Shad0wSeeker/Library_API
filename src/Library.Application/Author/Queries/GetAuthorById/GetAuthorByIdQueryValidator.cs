using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryValidator : AbstractValidator<GetAuthorByIdQuery>
    {
        public GetAuthorByIdQueryValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
