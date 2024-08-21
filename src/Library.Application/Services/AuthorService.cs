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
        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task UpdateAuthorAsync(int id, AuthorDto authorDto)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author != null)
            {
                _mapper.Map(authorDto, author);
                await _unitOfWork.Authors.UpdateAsync(author);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteAuthorAsync(int id)
        {
            await _unitOfWork.Authors.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
                   
    }
}
