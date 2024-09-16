using Library.Application.Book.Commands.CreateBookCommand;
using Library.Application.Book.Commands.DeleteBookCommand;
using Library.Application.Book.Commands.UpdateBookCommand;
using Library.Application.Book.Queries.GetBookById;
using Library.Application.DTOs;
using Library.Application.User.Commands.CreateUserCommand;
using Library.Application.User.Commands.DeleteUserCommand;
using Library.Application.User.Commands.UpdateUserCommand;
using Library.Application.User.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserRequestDto userDto, CancellationToken cancellationToken = default)
        {
            var command = new CreateUserCommand(userDto);
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, [FromBody] UserRequestDto userDto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateUserCommand(id, userDto);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
