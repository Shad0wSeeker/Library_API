using Library.Application.Book.Commands.DeleteBookCommand;
using Library.Domain.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Book.Commands
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly DeleteBookCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public DeleteBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteBookCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Library.Domain.Models.Book)null);

            var command = new DeleteBookCommand(1);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var book = new Library.Domain.Models.Book
            {
                Id = 1,
                ISBN = "1234567890",
                Name = "Book1",
                Genre = "Genre1",
                Description = "Description1",
                AuthorId = 1,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(10),
                ImagePath = "Path1"
            };

            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(book);

            _unitOfWorkMock.Setup(u => u.Books.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

            var command = new DeleteBookCommand(book.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
        }
    }
}
