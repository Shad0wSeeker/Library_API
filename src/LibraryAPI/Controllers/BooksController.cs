using Library.Application.DTOs;
using Library.Application.Interfaces;
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
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]  
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
        //[Authorize(Roles ="Admin")]
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
        //[Authorize]
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
        //[Authorize(Roles ="Admin")]
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
        //[Authorize(Roles ="Admin, Client")]
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
