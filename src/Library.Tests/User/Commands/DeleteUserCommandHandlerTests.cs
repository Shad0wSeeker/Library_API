using Library.Application.User.Commands.DeleteUserCommand;
using Library.Domain.Interfaces;
using Library.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.User.Commands
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly DeleteUserCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public DeleteUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteUserCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var command = new DeleteUserCommand(1);
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Library.Domain.Models.User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var existingUser = TestDataSeeder.GetUsers()[0];

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(existingUser);

            // Act
            await _handler.Handle(new DeleteUserCommand(existingUser.Id), CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.Users.DeleteAsync(existingUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
