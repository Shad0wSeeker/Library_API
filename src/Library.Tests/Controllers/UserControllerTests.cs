using Library.Application.DTOs;
using Library.Application.User.Commands.CreateUserCommand;
using Library.Application.User.Commands.UpdateUserCommand;
using Library.Application.User.Queries.GetUserById;
using Library.Tests.Data;
using LibraryAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var user = TestDataSeeder.GetUsers()[0];
            var userResponse = new UserResponseDto { Id = user.Id, Email = user.Email };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userResponse);

            // Act
            var result = await _controller.GetUserById(user.Id);

            // Assert
            var okResult = Assert.IsType<ActionResult<UserResponseDto>>(result);
            var actualResult = Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.Equal(200, actualResult.StatusCode);
            Assert.Equal(userResponse, actualResult.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedAtAction_WithCreatedUser()
        {
            // Arrange
            var userDto = new UserRequestDto { Email = "newuser@example.com" };
            var createdUser = new UserResponseDto { Id = 1, Email = userDto.Email };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdUser);

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            var createdAtActionResult = Assert.IsType<ActionResult<UserResponseDto>>(result);
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(createdAtActionResult.Result);
            Assert.Equal(201, createdAtAction.StatusCode);
            Assert.Equal(createdUser, createdAtAction.Value);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            var userDto = new UserRequestDto { Email = "updateduser@example.com" };
            var updatedUser = new UserResponseDto { Id = 1, Email = userDto.Email };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.UpdateUser(1, userDto);

            // Assert
            var okResult = Assert.IsType<ActionResult<UserResponseDto>>(result);
            var actualResult = Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.Equal(200, actualResult.StatusCode);
            Assert.Equal(updatedUser, actualResult.Value);
        }

    }
}
