using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetByEmailAndPasswordAsync(string email, string password);
    }
}
