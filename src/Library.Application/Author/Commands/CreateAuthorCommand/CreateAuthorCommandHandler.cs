using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;
using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Commands.CreateAuthorCommand
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthorResponseDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.AuthorRequest.AuthorFullName) || !request.AuthorRequest.AuthorFullName.Contains(' '))
            {
                throw new ArgumentException("AuthorFullName must contain at least a first name and a surname");
            }

            var author = _mapper.Map<Library.Domain.Models.Author>(request.AuthorRequest);
            await _unitOfWork.Authors.AddAsync(author, cancellationToken);

            return _mapper.Map<AuthorResponseDto>(author);
        }
    }
}
