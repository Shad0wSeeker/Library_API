using Library.Application.Author.Commands.DeleteAuthorCommand;
using Library.Domain.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Author.Commands
{
    public class DeleteAuthorCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteAuthorCommandHandler _handler;

        public DeleteAuthorCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteAuthorCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidId_DeletesAuthor()
        {
            // Arrange
            var authorId = 1;
            _mockUnitOfWork.Setup(uow => uow.Authors.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Library.Domain.Models.Author { Id = authorId });
            _mockUnitOfWork.Setup(uow => uow.Authors.DeleteAsync(authorId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new DeleteAuthorCommand(authorId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Authors.DeleteAsync(authorId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidId_ThrowsInvalidOperationException()
        {
            // Arrange
            var authorId = 999;
            _mockUnitOfWork.Setup(uow => uow.Authors.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Library.Domain.Models.Author)null);

            var command = new DeleteAuthorCommand(authorId);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
