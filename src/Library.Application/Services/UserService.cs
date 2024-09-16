using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return _mapper.Map<UserResponseDto>(user);
        }
        public async Task<UserResponseDto> CreateUserAsync(UserRequestDto userDto, CancellationToken cancellationToken = default)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(userDto.Email, cancellationToken);
            
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with such email already exists.");

            }

            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int id, UserRequestDto userDto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            _mapper.Map(userDto, user);
            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);            

            return _mapper.Map<UserResponseDto>(user);
        }


        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            await _unitOfWork.Users.DeleteAsync(id, cancellationToken);

        }

        public async Task<User> AuthenticateAsync(string email, /*string password,*/ CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken);
            return user;
        }
    }
}
