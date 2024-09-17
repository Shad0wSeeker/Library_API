using FluentValidation;
using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryValidator: AbstractValidator<GetAllAuthorsQuery> 
    {
        public GetAllAuthorsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).NotEmpty();
            RuleFor(x => x.PageSize).GreaterThan(0).NotEmpty();
        }
    }
}
