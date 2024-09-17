using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Queries.GetAllBooks
{
    public class GetAllBooksQueryValidator : AbstractValidator<GetAllBooksQuery>
    {
        public GetAllBooksQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).NotEmpty();
            RuleFor(x => x.PageSize).GreaterThan(0).NotEmpty();
        }
    }
}
