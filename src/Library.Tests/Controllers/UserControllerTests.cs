using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Domain.Models;
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
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUserDto()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto { Id = userId, Email = "test@example.com" };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnValue.Id);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((UserDto)null);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedAtAction_WithUserDto()
        {
            // Arrange
            var userDto = new UserDto { Id = 1, Email = "test@example.com" };
            _userServiceMock.Setup(s => s.CreateUserAsync(userDto)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.CreateUser(userDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<UserDto>(createdResult.Value);
            Assert.Equal(userDto.Id, returnValue.Id);
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _controller.CreateUser(new UserDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult_WithUpdatedUserDto()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto { Id = userId, Email = "updated@example.com" };
            var updatedUserDto = new UserDto { Id = userId, Email = "updated@example.com" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(userId, userDto)).ReturnsAsync(updatedUserDto);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(updatedUserDto.Email, returnValue.Email);
        }

        [Fact]
        public async Task UpdateUser_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto { Id = 2 }; // Id does not match

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto { Id = userId };
            _userServiceMock.Setup(s => s.UpdateUserAsync(userId, userDto)).ReturnsAsync((UserDto)null);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        
        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((UserDto)null);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
