using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public UserServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsMappedUserDto_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Email = "test@example.com", Password = "password" };
            var userDto = new UserDto { Id = userId, Email = "test@example.com" };

            _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            _unitOfWorkMock.Verify(uow => uow.Users.GetByIdAsync(userId), Times.Once);
            _mapperMock.Verify(m => m.Map<UserDto>(user), Times.Once);
        }
        
        [Fact]
        public async Task UpdateUserAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto { Id = userId, Email = "updated@example.com" };

            _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.UpdateUserAsync(userId, userDto);

            // Assert
            Assert.Null(result);
            _unitOfWorkMock.Verify(uow => uow.Users.GetByIdAsync(userId), Times.Once);
            _mapperMock.Verify(m => m.Map(userDto, It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Users.UpdateAsync(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_CallsDeleteAndComplete()
        {
            // Arrange
            var userId = 1;

            _unitOfWorkMock.Setup(uow => uow.Users.DeleteAsync(userId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Users.DeleteAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsUser_WhenCredentialsMatch()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            var user = new User { Id = 1, Email = email, Password = password };

            _unitOfWorkMock.Setup(uow => uow.Users.GetByEmailAndPasswordAsync(email, password)).ReturnsAsync(user);

            // Act
            var result = await _userService.AuthenticateAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
            _unitOfWorkMock.Verify(uow => uow.Users.GetByEmailAndPasswordAsync(email, password), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsNull_WhenCredentialsDoNotMatch()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongpassword";

            _unitOfWorkMock.Setup(uow => uow.Users.GetByEmailAndPasswordAsync(email, password)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.AuthenticateAsync(email, password);

            // Assert
            Assert.Null(result);
            _unitOfWorkMock.Verify(uow => uow.Users.GetByEmailAndPasswordAsync(email, password), Times.Once);
        }
    }
}