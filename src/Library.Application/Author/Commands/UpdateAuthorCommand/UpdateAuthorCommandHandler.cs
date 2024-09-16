using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Commands.UpdateAuthorCommand
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthorResponseDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(request.Id, cancellationToken);
            if (author == null)
            {
                throw new InvalidOperationException("Author not found.");
            }

            _mapper.Map(request.AuthorRequest, author);
            await _unitOfWork.Authors.UpdateAsync(author, cancellationToken);

            return _mapper.Map<AuthorResponseDto>(author);
        }
    }
}
