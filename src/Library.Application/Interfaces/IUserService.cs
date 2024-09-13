using Library.Application.DTOs;
using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<UserDto> CreateUserAsync(UserDto userDto, CancellationToken cancellationToken = default);
        Task<UserDto> UpdateUserAsync(int id, UserDto userDto, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
        Task<User> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}
