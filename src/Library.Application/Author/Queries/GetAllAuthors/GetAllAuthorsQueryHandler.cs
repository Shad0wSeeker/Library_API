using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using Library.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Author.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, PaginatedResultDto<AuthorResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAuthorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResultDto<AuthorResponseDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var paginatedAuthors = await _unitOfWork.Authors.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                query => query.Include(a => a.Books),
                cancellationToken
            );

            var authorDtos = _mapper.Map<IEnumerable<AuthorResponseDto>>(paginatedAuthors.Items);

            return new PaginatedResultDto<AuthorResponseDto>(authorDtos, paginatedAuthors.TotalCount, request.PageSize, request.PageNumber);
        }
    }
}
