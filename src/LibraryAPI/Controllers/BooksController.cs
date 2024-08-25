using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllBooks(int pageNumber = 1, int pageSize = 3)
        {
            var result = await _bookService.GetAllBooksAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminAndClientPolicy")]
        public async Task<ActionResult<BookDto>> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("isbn/{isbn}")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<ActionResult<BookDto>> GetBookByISBNAsync(string isbn)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn);
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
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdBook = await _bookService.CreateBookAsync(bookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest("ID mismatched");
            }

            var updatedBook = await _bookService.UpdateBookAsync(id, bookDto);

            if(updatedBook == null)
            {
                return NotFound();
            }
            return Ok(updatedBook);
        }

        [HttpDelete]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<BookDto>> DeleteBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book==null)
            {
                return NotFound();
            }
            await _bookService.DeleteBookAsync(id);
            return NoContent();

        }

        [HttpPost("borrow")]
        [Authorize(Policy = "ClientPolicy")]
        public async Task<IActionResult> BorrowBook([FromBody]BorrowBookDto borrowBookDto)
        {
            var borrowedBook = await _bookService.BorrowBookAsync(borrowBookDto);

            if (borrowedBook == null)
            {
                return NotFound("Book or User not found");
            }

            return Ok(borrowedBook);
        }



    }
}
