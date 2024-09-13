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
        public async Task<ActionResult<AuthorDto>> GetAuthorById(int id, CancellationToken cancellationToken = default)
        {
            var author = await _authorService.GetAuthorByIdAsync(id, cancellationToken);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<AuthorDto>> CreateAuthor([FromBody] AuthorDto authorDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAuthor = await _authorService.CreateAuthorAsync(authorDto, cancellationToken);
            return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(int id, [FromBody] AuthorDto authorDto, CancellationToken cancellationToken = default)
        {
            if (id != authorDto.Id)
            {
                return BadRequest("ID mismatched");
            }

            var updatedAuthor = await _authorService.UpdateAuthorAsync(id, authorDto, cancellationToken);

            if (updatedAuthor == null)
            {
                return NotFound();
            }
            return Ok(updatedAuthor);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken cancellationToken = default)
        {
            var author = await _authorService.GetAuthorByIdAsync(id, cancellationToken);
            if (author == null)
            {
                return NotFound();
            }
            await _authorService.DeleteAuthorAsync(id, cancellationToken);
            return Ok(new { message = "Author deleted successfully" });

        }

        
    }
}
