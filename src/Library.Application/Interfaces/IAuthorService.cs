using Library.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto> GetAuthorByIdAsync(int id);
        Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(int id, AuthorDto authorDto);
        Task DeleteAuthorAsync(int id);
    }
}
