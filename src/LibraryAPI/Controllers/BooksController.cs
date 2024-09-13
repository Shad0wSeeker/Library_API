using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminAndClientPolicy")]
        public async Task<IActionResult> GetAllBooks(int pageNumber = 1, int pageSize = 3, CancellationToken cancellationToken = default)
        {
            var result = await _bookService.GetAllBooksAsync(pageNumber, pageSize, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminAndClientPolicy")]
        public async Task<ActionResult<BookDto>> GetBookById(int id, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.GetBookByIdAsync(id, cancellationToken);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("isbn/{isbn}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<ActionResult<BookDto>> GetBookByISBN(string isbn, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn, cancellationToken);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        /// <summary>
        /// Creates a book.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto bookDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdBook = await _bookService.CreateBookAsync(bookDto, cancellationToken);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] BookDto bookDto, CancellationToken cancellationToken = default)
        {
            if (id != bookDto.Id)
            {
                return BadRequest("ID mismatched");
            }

            var updatedBook = await _bookService.UpdateBookAsync(id, bookDto, cancellationToken);

            if (updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.GetBookByIdAsync(id, cancellationToken);
            if (book == null)
            {
                return NotFound();
            }
            await _bookService.DeleteBookAsync(id, cancellationToken);
            return Ok(new { message = "Book deleted successfully" });

        }

        [HttpPost("borrow")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookDto borrowBookDto, CancellationToken cancellationToken = default)
        {
            var borrowedBook = await _bookService.BorrowBookAsync(borrowBookDto, cancellationToken);

            if (borrowedBook == null)
            {
                return NotFound("Book or User not found");
            }

            return Ok(borrowedBook);
        }

        
    
    }
}
