using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {

        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }


        [HttpGet]
        [Authorize(Policy = "AdminAndClientPolicy")]
        public async Task<IActionResult> GetAllAuthors(int pageNumber = 1, int pageSize = 3, CancellationToken cancellationToken = default)
        {
            var result = await _authorService.GetAllAuthorsAsync(pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<ActionResult<AuthorResponseDto>> GetAuthorById(int id, CancellationToken cancellationToken = default)
        {
            var author = await _authorService.GetAuthorByIdAsync(id, cancellationToken);           
            return Ok(author);
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<AuthorResponseDto>> CreateAuthor([FromBody] AuthorRequestDto authorDto, CancellationToken cancellationToken = default)
        {
            var createdAuthor = await _authorService.CreateAuthorAsync(authorDto, cancellationToken);
            return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<AuthorResponseDto>> UpdateAuthor(int id, [FromBody] AuthorRequestDto authorDto, CancellationToken cancellationToken = default)
        {            
            var updatedAuthor = await _authorService.UpdateAuthorAsync(id, authorDto, cancellationToken);            
            return Ok(updatedAuthor);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken cancellationToken = default)
        {            
            await _authorService.DeleteAuthorAsync(id, cancellationToken);
            return Ok(new { message = "Author deleted successfully" });

        }
               
    }
}
