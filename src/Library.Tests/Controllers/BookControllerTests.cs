using Library.Application.Book.Commands.BorrowBookCommand;
using Library.Application.Book.Commands.CreateBookCommand;
using Library.Application.Book.Commands.DeleteBookCommand;
using Library.Application.Book.Commands.UpdateBookCommand;
using Library.Application.Book.Queries.GetAllBooks;
using Library.Application.Book.Queries.GetBookById;
using Library.Application.Book.Queries.GetBookByISBN;
using Library.Application.DTOs;
using Library.Shared.DTO;
using Library.Tests.Data;
using LibraryAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace Library.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithPaginatedBooks()
        {
            // Arrange
            var books = TestDataSeeder.GetBooks();
            var bookResponseDtos = books.Select(b => new BookResponseDto { Id = b.Id, Name = b.Name, ISBN = b.ISBN }).ToList();
            var paginatedBooks = new PaginatedResultDto<BookResponseDto>(bookResponseDtos, books.Count, 1, 3);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBooksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedBooks);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(paginatedBooks, okResult.Value);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult_WithBook()
        {
            // Arrange
            var book = TestDataSeeder.GetBooks()[0];
            var bookResponse = new BookResponseDto { Id = book.Id, Name = book.Name, ISBN = book.ISBN };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookResponse);

            // Act
            var result = await _controller.GetBookById(book.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(bookResponse, okResult.Value);
        }

        [Fact]
        public async Task GetBookByISBN_ReturnsOkResult_WithBook()
        {
            // Arrange
            var book = TestDataSeeder.GetBooks()[0];
            var bookResponse = new BookResponseDto { ISBN = book.ISBN, Name = book.Name };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetBookByISBNQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookResponse);

            // Act
            var result = await _controller.GetBookByISBN(book.ISBN);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(bookResponse, okResult.Value);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedAtAction_WithCreatedBook()
        {
            // Arrange
            var bookDto = new BookResponseDto { Id = 1, Name = "New Book" };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookDto);

            // Act
            var result = await _controller.CreateBook(new BookRequestDto());

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(bookDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOkResult_WithUpdatedBook()
        {
            // Arrange
            var bookDto = new BookResponseDto { Id = 1, Name = "Updated Book" };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookDto);

            // Act
            var result = await _controller.UpdateBook(1, new BookRequestDto());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(bookDto, okResult.Value);
        }       

    }
}
