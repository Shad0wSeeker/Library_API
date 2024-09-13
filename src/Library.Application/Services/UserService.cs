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

        public async Task<UserDto> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> CreateUserAsync(UserDto userDto, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserDto userDto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                return null; 
            }

            _mapper.Map(userDto, user);

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            

            return _mapper.Map<UserDto>(user);
        }


        public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Users.DeleteAsync(id, cancellationToken);
            
        }

        public async Task<User> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.Users.GetByEmailAndPasswordAsync(email, password, cancellationToken);
            return user;
        }
    }
}
