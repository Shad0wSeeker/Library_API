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
        public async Task<ActionResult<BookResponseDto>> GetBookById(int id, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.GetBookByIdAsync(id, cancellationToken);            
            return Ok(book);
        }

        [HttpGet("isbn/{isbn}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<ActionResult<BookResponseDto>> GetBookByISBN(string isbn, CancellationToken cancellationToken = default)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn, cancellationToken);           
            return Ok(book);
        }

        /// <summary>
        /// Creates a book.
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookResponseDto>> CreateBook([FromBody] BookRequestDto bookDto, CancellationToken cancellationToken = default)
        {
            var createdBook = await _bookService.CreateBookAsync(bookDto, cancellationToken);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookResponseDto>> UpdateBook(int id, [FromBody] BookRequestDto bookDto, CancellationToken cancellationToken = default)
        {            
            var updatedBook = await _bookService.UpdateBookAsync(id, bookDto, cancellationToken);
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken = default)
        {            
            await _bookService.DeleteBookAsync(id, cancellationToken);
            return Ok(new { message = "Book deleted successfully" });

        }

        [HttpPost("borrow")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookDto borrowBookDto, CancellationToken cancellationToken = default)
        {
            var borrowedBook = await _bookService.BorrowBookAsync(borrowBookDto, cancellationToken);
            return Ok(borrowedBook);
        }     
    
    }
}
