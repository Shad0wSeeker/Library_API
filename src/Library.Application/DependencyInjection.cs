using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation.AspNetCore;
using Library.Application.Author.Commands.CreateAuthorCommand;
using Library.Application.Author.Commands.DeleteAuthorCommand;
using Library.Application.Author.Commands.UpdateAuthorCommand;
using Library.Application.Author.Queries.GetAllAuthors;
using Library.Application.Author.Queries.GetAuthorById;
using Library.Application.Book.Commands.BorrowBookCommand;
using Library.Application.Book.Commands.CreateBookCommand;
using Library.Application.Book.Commands.DeleteBookCommand;
using Library.Application.Book.Commands.UpdateBookCommand;
using Library.Application.Book.Queries.GetAllBooks;
using Library.Application.Book.Queries.GetBookById;
using Library.Application.Book.Queries.GetBookByISBN;
using Library.Application.User.Commands.CreateUserCommand;
using Library.Application.User.Commands.DeleteUserCommand;
using Library.Application.User.Commands.UpdateUserCommand;
using Library.Application.User.Queries.AuthenticateUser;
using Library.Application.User.Queries.GetUserById;

namespace Library.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
