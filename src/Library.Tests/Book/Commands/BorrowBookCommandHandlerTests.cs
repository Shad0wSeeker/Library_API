using Library.Application.Book.Commands.BorrowBookCommand;
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
    public class BorrowBookCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BorrowBookCommandHandler _handler;

        public BorrowBookCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new BorrowBookCommandHandler(_mockUnitOfWork.Object);
        }

        
        [Fact]
        public async Task Handle_ShouldThrowException_WhenBookAlreadyBorrowed()
        {
            // Arrange
            var book = new Library.Domain.Models.Book
            {
                Id = 1,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(7)
            };

            var user = new Library.Domain.Models.User
            {
                Id = 1,
                BorrowedBooks = new List<Library.Domain.Models.Book>()
            };

            var command = new BorrowBookCommand(1, 1, DateTime.Now, DateTime.Now.AddDays(7));

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(book.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _mockUnitOfWork.Setup(uow => uow.Users.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenReturningTimeIsBeforeBorrowingTime()
        {
            // Arrange
            var book = new Library.Domain.Models.Book
            {
                Id = 1,
                BorrowingTime = default,
                ReturningTime = default
            };

            var user = new Library.Domain.Models.User
            {
                Id = 1,
                BorrowedBooks = new List<Library.Domain.Models.Book>()
            };

            var command = new BorrowBookCommand(1, 1, DateTime.Now, DateTime.Now.AddDays(-7));

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(book.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _mockUnitOfWork.Setup(uow => uow.Users.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
        
    }
}
