using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Shared.DTO;
using LibraryAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _controller = new BooksController(_bookServiceMock.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithPaginatedBooks()
        {
            // Arrange
            var books = new PaginatedResultDto<BookDto>(new List<BookDto>(), 0, 1, 1);
            _bookServiceMock.Setup(s => s.GetAllBooksAsync(1, 3)).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAllBooks(1, 3);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(books, okResult.Value);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult_WithBook()
        {
            // Arrange
            var book = new BookDto { Id = 1, Name = "Book 1" };
            _bookServiceMock.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBookById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(book, okResult.Value);
        }

        [Fact]
        public async Task GetBookById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync((BookDto)null);

            // Act
            var result = await _controller.GetBookById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetBookByISBN_ReturnsOkResult_WithBook()
        {
            // Arrange
            var book = new BookDto { Id = 1, Name = "Book 1", ISBN = "1234567890" };
            _bookServiceMock.Setup(s => s.GetBookByISBNAsync("1234567890")).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBookByISBN("1234567890");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(book, okResult.Value);
        }

        [Fact]
        public async Task GetBookByISBN_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.GetBookByISBNAsync("1234567890")).ReturnsAsync((BookDto)null);

            // Act
            var result = await _controller.GetBookByISBN("1234567890");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedAtAction_WithCreatedBook()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Name = "New Book" };
            _bookServiceMock.Setup(s => s.CreateBookAsync(bookDto)).ReturnsAsync(bookDto);

            // Act
            var result = await _controller.CreateBook(bookDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(bookDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOkResult_WithUpdatedBook()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Name = "Updated Book" };
            _bookServiceMock.Setup(s => s.UpdateBookAsync(1, bookDto)).ReturnsAsync(bookDto);

            // Act
            var result = await _controller.UpdateBook(1, bookDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(bookDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Name = "Updated Book" };
            _bookServiceMock.Setup(s => s.UpdateBookAsync(1, bookDto)).ReturnsAsync((BookDto)null);

            // Act
            var result = await _controller.UpdateBook(1, bookDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent_WhenBookIsDeleted()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync(new BookDto { Id = 1 });
            _bookServiceMock.Setup(s => s.DeleteBookAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBook(1);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.GetBookByIdAsync(1)).ReturnsAsync((BookDto)null);

            // Act
            var result = await _controller.DeleteBook(1);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
