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
        public async Task<IActionResult> GetAllAuthors(int pageNumber = 1, int pageSize = 3)
        {
            var result = await _authorService.GetAllAuthorsAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<ActionResult<AuthorDto>> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<AuthorDto>> CreateAuthor([FromBody] AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAuthor = await _authorService.CreateAuthorAsync(authorDto);
            return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(int id, [FromBody] AuthorDto authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest("ID mismatched");
            }

            var updatedAuthor = await _authorService.UpdateAuthorAsync(id, authorDto);

            if (updatedAuthor == null)
            {
                return NotFound();
            }
            return Ok(updatedAuthor);
        }

        [HttpDelete]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<AuthorDto>> DeleteAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            await _authorService.DeleteAuthorAsync(id);
            return NoContent();

        }

        
    }
}
