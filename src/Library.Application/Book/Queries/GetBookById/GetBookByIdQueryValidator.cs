using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Queries.GetBookById
{
    public class GetBookByIdQueryValidator : AbstractValidator<GetBookByIdQuery>
    {
        public GetBookByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
