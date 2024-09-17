using AutoMapper;
using Library.Application.Book.Commands.CreateBookCommand;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Domain.Interfaces;
using Library.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Book.Commands
{
    public class CreateBookCommandHandlerTests
    {
        private readonly CreateBookCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public CreateBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new CreateBookCommandHandler(_mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenBookWithISBNAlreadyExists()
        {
            // Arrange
            var existingBook = TestDataSeeder.GetBooks().First();
            _unitOfWorkMock.Setup(u => u.Books.GetByISBNAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(existingBook);

            var bookRequest = new BookRequestDto
            {
                ISBN = existingBook.ISBN,
                Name = "New Book",
                Genre = "New Genre",
                Description = "New Description",
                AuthorId = 1,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(10),
                ImagePath = "New Path"
            };

            var command = new CreateBookCommand(bookRequest);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnBookResponseDto_WhenBookIsValid()
        {
            // Arrange
            var bookRequest = new BookRequestDto
            {
                ISBN = "1122334455",
                Name = "New Book",
                Genre = "New Genre",
                Description = "New Description",
                AuthorId = 1,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(10),
                ImagePath = "New Path"
            };

            var book = new Library.Domain.Models.Book
            {
                Id = 1,
                ISBN = bookRequest.ISBN,
                Name = bookRequest.Name,
                Genre = bookRequest.Genre,
                Description = bookRequest.Description,
                AuthorId = bookRequest.AuthorId,
                BorrowingTime = bookRequest.BorrowingTime,
                ReturningTime = bookRequest.ReturningTime,
                ImagePath = bookRequest.ImagePath
            };

            _unitOfWorkMock.Setup(u => u.Books.GetByISBNAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Library.Domain.Models.Book)null);

            _unitOfWorkMock.Setup(u => u.Books.AddAsync(It.IsAny<Library.Domain.Models.Book>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(book);

            var command = new CreateBookCommand(bookRequest);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookRequest.ISBN, result.ISBN);
            Assert.Equal(bookRequest.Name, result.Name);
            Assert.Equal(bookRequest.Genre, result.Genre);
            Assert.Equal(bookRequest.Description, result.Description);
            Assert.Equal(bookRequest.AuthorId, result.AuthorId);
            Assert.Equal(bookRequest.ImagePath, result.ImagePath);
        }
    }
}
