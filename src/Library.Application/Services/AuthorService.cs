using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Models;
using Library.Shared.DTO;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResultDto<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var paginatedAuthors = await _unitOfWork.Authors.GetAllAsync(
                pageNumber,
                pageSize,
                query => query.Include(a => a.Books),
                cancellationToken
            );

            var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(paginatedAuthors.Items);

            return new PaginatedResultDto<AuthorDto>(authorDtos, paginatedAuthors.TotalCount, pageSize, pageNumber);
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, cancellationToken);
            if (author == null)
            {
                throw new InvalidOperationException("Author not found.");
            }
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto, CancellationToken cancellationToken = default)
        {
            var existingAuthor = await _unitOfWork.Authors.GetByIdAsync(authorDto.Id, cancellationToken);
            
            if (existingAuthor != null)
            {
                throw new InvalidOperationException("An author with this ID already exists.");
            }

            var author = _mapper.Map<Author>(authorDto);
            await _unitOfWork.Authors.AddAsync(author, cancellationToken);
            
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(int id, AuthorDto authorDto, CancellationToken cancellationToken = default)
        {
            if (id != authorDto.Id)
            {
                throw new ArgumentException("ID mismatched.");
            }
            var author = await _unitOfWork.Authors.GetByIdAsync(id, cancellationToken);

            if (author == null)
            {
                return null; 
            }

            _mapper.Map(authorDto, author);

            await _unitOfWork.Authors.UpdateAsync(author, cancellationToken);
            

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, cancellationToken);

            if (author == null)
            {
                throw new InvalidOperationException("Author not found");
            }

            await _unitOfWork.Authors.DeleteAsync(id, cancellationToken);
                     
        }
                   
    }
}
