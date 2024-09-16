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
        Task<UserResponseDto> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<UserResponseDto> CreateUserAsync(UserRequestDto userDto, CancellationToken cancellationToken = default);
        Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto userDto, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
        Task<User> AuthenticateAsync(string email, CancellationToken cancellationToken = default);
    }
}
