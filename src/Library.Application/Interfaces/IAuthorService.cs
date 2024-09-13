﻿using Library.Application.DTOs;
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
        Task<PaginatedResultDto<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<AuthorDto> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto, CancellationToken cancellationToken = default);
        Task<AuthorDto> UpdateAuthorAsync(int id, AuthorDto authorDto, CancellationToken cancellationToken = default);
        Task DeleteAuthorAsync(int id, CancellationToken cancellationToken = default);
    }
}
