using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Books = new BookRepository(_context);
            Authors = new AuthorRepository(_context);
            Users = new UserRepository(_context);
        }
        public IBookRepository Books { get; private set; }
        public IAuthorRepository Authors { get; private set; }
        public IUserRepository Users { get; private set; }
                
    }
}
