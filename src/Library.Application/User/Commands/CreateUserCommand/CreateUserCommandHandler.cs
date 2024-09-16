using AutoMapper;
using Library.Application.Book.Commands.CreateBookCommand;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.User.Commands.CreateUserCommand
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.UserRequest.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with such email already exists.");
            }

            var user = _mapper.Map<Library.Domain.Models.User>(request.UserRequest);
            await _unitOfWork.Users.AddAsync(user, cancellationToken);

            return _mapper.Map<UserResponseDto>(user);

        }
    }
}