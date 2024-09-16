using Library.Application.Book.Commands.BorrowBookCommand;
using Library.Application.Book.Commands.CreateBookCommand;
using Library.Application.Book.Commands.DeleteBookCommand;
using Library.Application.Book.Commands.UpdateBookCommand;
using Library.Application.Book.Queries.GetAllBooks;
using Library.Application.Book.Queries.GetBookById;
using Library.Application.Book.Queries.GetBookByISBN;
using Library.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "AdminAndClientPolicy")]
        public async Task<IActionResult> GetAllBooks(int pageNumber = 1, int pageSize = 3, CancellationToken cancellationToken = default)
        {
            var query = new GetAllBooksQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminAndClientPolicy")]
        public async Task<ActionResult<BookResponseDto>> GetBookById(int id, CancellationToken cancellationToken = default)
        {
            var query = new GetBookByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("isbn/{isbn}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<ActionResult<BookResponseDto>> GetBookByISBN(string isbn, CancellationToken cancellationToken = default)
        {
            var query = new GetBookByISBNQuery(isbn);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Creates a book.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookResponseDto>> CreateBook([FromBody] BookRequestDto bookDto, CancellationToken cancellationToken = default)
        {
            var command = new CreateBookCommand(bookDto);
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetBookById), new { id = result.Id }, result);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookResponseDto>> UpdateBook(int id, [FromBody] BookRequestDto bookDto, CancellationToken cancellationToken = default)
        {
            var command = new UpdateBookCommand(id, bookDto);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteBookCommand(id);
            await _mediator.Send(command, cancellationToken);
            return Ok(new { message = "Book deleted successfully" });
        }

        [HttpPost("borrow")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookDto borrowBookDto, CancellationToken cancellationToken = default)
        {
            var command = new BorrowBookCommand(
                borrowBookDto.BookId,
                borrowBookDto.UserId,
                borrowBookDto.BorrowingTime,
                borrowBookDto.ReturningTime);

            var borrowedBook = await _mediator.Send(command, cancellationToken);
            return Ok(borrowedBook);
        }

    }
}
