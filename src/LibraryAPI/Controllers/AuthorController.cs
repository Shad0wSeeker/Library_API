using Library.Application.Author.Commands.CreateAuthorCommand;
using Library.Application.Author.Commands.DeleteAuthorCommand;
using Library.Application.Author.Commands.UpdateAuthorCommand;
using Library.Application.Author.Queries.GetAllAuthors;
using Library.Application.Author.Queries.GetAuthorById;
using Library.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [Authorize(Policy = "AdminAndClientPolicy")]
        public async Task<IActionResult> GetAllAuthors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3, CancellationToken cancellationToken = default)
        {
            var query = new GetAllAuthorsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> GetAuthorById(int id, CancellationToken cancellationToken = default)
        {
            var query = new GetAuthorByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorRequestDto authorRequestDto, CancellationToken cancellationToken = default)
        {
            var command = new CreateAuthorCommand(authorRequestDto);
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAuthorById), new { id = result.Id }, result);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorRequestDto authorRequestDto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateAuthorCommand(id, authorRequestDto);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteAuthor(int id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteAuthorCommand(id);
            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Author deleted successfully" });
        }

    }
}
