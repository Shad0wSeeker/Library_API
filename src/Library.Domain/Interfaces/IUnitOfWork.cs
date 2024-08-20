using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; } 
        IAuthorRepository Authors { get; }
        IUserRepository Users { get; }
        Task<int> CompleteAsync();
    }
}
