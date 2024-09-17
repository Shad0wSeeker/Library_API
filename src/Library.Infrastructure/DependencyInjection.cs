using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("SqliteConnection");
            string dataDirectory = String.Empty;
            connStr = String.Format(connStr, dataDirectory);

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connStr).Options;
            services.AddScoped<AppDbContext>(s => new AppDbContext(options));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
