using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Shared.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsPaginatedResultDto()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Book 1", ISBN = "1234567890" },
                new Book { Id = 2, Name = "Book 2", ISBN = "0987654321" }
            };

            var paginatedBooks = new PaginatedResultDto<Book>(books, books.Count, 1, 10);

            _unitOfWorkMock.Setup(u => u.Books.GetAllAsync(1, 10)).ReturnsAsync(paginatedBooks);

            var bookDtos = new List<BookDto>
            {
                new BookDto { Id = 1, Name = "Book 1", ISBN = "1234567890" },
                new BookDto { Id = 2, Name = "Book 2", ISBN = "0987654321" }
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(books)).Returns(bookDtos);

            // Act
            var result = await _bookService.GetAllBooksAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
        }

        [Fact]
        public async Task CreateBookAsync_CreatesBook_ReturnsBookDto()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Name = "Book 1", ISBN = "1234567890" };
            var book = new Book { Id = 1, Name = "Book 1", ISBN = "1234567890" };

            _mapperMock.Setup(m => m.Map<Book>(bookDto)).Returns(book);
            _unitOfWorkMock.Setup(u => u.Books.AddAsync(book)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(bookDto);

            // Act
            var result = await _bookService.CreateBookAsync(bookDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesBook_ReturnsUpdatedBookDto()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Name = "Updated Book", ISBN = "1234567890" };
            var book = new Book { Id = 1, Name = "Book 1", ISBN = "1234567890" };

            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map(bookDto, book)).Returns(book);
            _unitOfWorkMock.Setup(u => u.Books.UpdateAsync(book)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(bookDto);

            // Act
            var result = await _bookService.UpdateBookAsync(1, bookDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Book", result.Name);
        }

        [Fact]
        public async Task DeleteBookAsync_DeletesBook()
        {
            // Arrange
            var bookId = 1;

            _unitOfWorkMock.Setup(u => u.Books.DeleteAsync(bookId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _unitOfWorkMock.Verify(u => u.Books.DeleteAsync(bookId), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
