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
        public async Task<PaginatedResultDto<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize)
        {
            var paginatedAuthors = await _unitOfWork.Authors.GetAllAsync(pageNumber, pageSize);
            var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(paginatedAuthors.Items);

            return new PaginatedResultDto<AuthorDto>(authorDtos, paginatedAuthors.TotalCount, pageSize, pageNumber);
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

        public async Task<AuthorDto> UpdateAuthorAsync(int id, AuthorDto authorDto)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
            {
                return null; // Автор не найден
            }

            // Обновляем данные автора с помощью AutoMapper
            _mapper.Map(authorDto, author);

            // Обновляем автора в базе данных
            await _unitOfWork.Authors.UpdateAsync(author);
            await _unitOfWork.CompleteAsync();

            // Возвращаем обновленные данные автора
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task DeleteAuthorAsync(int id)
        {
            await _unitOfWork.Authors.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
                   
    }
}
