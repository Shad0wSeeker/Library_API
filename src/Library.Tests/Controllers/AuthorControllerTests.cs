using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Shared.DTO;
using LibraryAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly Mock<IAuthorService> _authorServiceMock;
        private readonly AuthorController _controller;

        public AuthorControllerTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _controller = new AuthorController(_authorServiceMock.Object);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsOkResult_WithPaginatedAuthors()
        {
            // Arrange
            var authors = new PaginatedResultDto<AuthorDto>(new List<AuthorDto>(), 0, 1, 1);
            _authorServiceMock.Setup(s => s.GetAllAuthorsAsync(1, 3)).ReturnsAsync(authors);

            // Act
            var result = await _controller.GetAllAuthors(1, 3);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(authors, okResult.Value);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsOkResult_WithAuthor()
        {
            // Arrange
            var author = new AuthorDto { Id = 1, AuthorFullName = "Author 1" };
            _authorServiceMock.Setup(s => s.GetAuthorByIdAsync(1)).ReturnsAsync(author);

            // Act
            var result = await _controller.GetAuthorById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(author, okResult.Value);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            _authorServiceMock.Setup(s => s.GetAuthorByIdAsync(1)).ReturnsAsync((AuthorDto)null);

            // Act
            var result = await _controller.GetAuthorById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAtAction_WithCreatedAuthor()
        {
            // Arrange
            var authorDto = new AuthorDto { Id = 1, AuthorFullName = "New Author" };
            _authorServiceMock.Setup(s => s.CreateAuthorAsync(authorDto)).ReturnsAsync(authorDto);

            // Act
            var result = await _controller.CreateAuthor(authorDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(authorDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsOkResult_WithUpdatedAuthor()
        {
            // Arrange
            var authorDto = new AuthorDto { Id = 1, AuthorFullName = "Updated Author" };
            _authorServiceMock.Setup(s => s.UpdateAuthorAsync(1, authorDto)).ReturnsAsync(authorDto);

            // Act
            var result = await _controller.UpdateAuthor(1, authorDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(authorDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorDto = new AuthorDto { Id = 1, AuthorFullName = "Updated Author" };
            _authorServiceMock.Setup(s => s.UpdateAuthorAsync(1, authorDto)).ReturnsAsync((AuthorDto)null);

            // Act
            var result = await _controller.UpdateAuthor(1, authorDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsNoContent_WhenAuthorIsDeleted()
        {
            // Arrange
            _authorServiceMock.Setup(s => s.GetAuthorByIdAsync(1)).ReturnsAsync(new AuthorDto { Id = 1 });
            _authorServiceMock.Setup(s => s.DeleteAuthorAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAuthor(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            _authorServiceMock.Setup(s => s.GetAuthorByIdAsync(1)).ReturnsAsync((AuthorDto)null);

            // Act
            var result = await _controller.DeleteAuthor(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
