using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Queries.GetBookByISBN
{
    public class GetBookByISBNQueryValidator : AbstractValidator<GetBookByISBNQuery>
    {
        public GetBookByISBNQueryValidator()
        {
            RuleFor(x => x.ISBN).NotEmpty();
        }
    }
}
