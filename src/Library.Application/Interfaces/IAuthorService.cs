using Library.Application.DTOs;
using Library.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<PaginatedResultDto<AuthorResponseDto>> GetAllAuthorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<AuthorResponseDto> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AuthorResponseDto> CreateAuthorAsync(AuthorRequestDto authorDto, CancellationToken cancellationToken = default);
        Task<AuthorResponseDto> UpdateAuthorAsync(int id, AuthorRequestDto authorDto, CancellationToken cancellationToken = default);
        Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default);
    }
}
