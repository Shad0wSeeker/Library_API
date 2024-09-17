using Library.Application.Author.Commands.CreateAuthorCommand;
using Library.Application.Author.Commands.UpdateAuthorCommand;
using Library.Application.Author.Queries.GetAllAuthors;
using Library.Application.Author.Queries.GetAuthorById;
using Library.Application.DTOs;
using Library.Shared.DTO;
using Library.Tests.Data;
using LibraryAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthorController _controller;

        public AuthorControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthorController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsOkResult_WithPaginatedAuthors()
        {
            // Arrange
            var authors = TestDataSeeder.GetAuthors();
            var authorDtos = authors.Select(a => new AuthorResponseDto
            {
                Id = a.Id,
                AuthorFullName = $"{a.Name} {a.Surname}",
                DateOfBirth = a.DateOfBirth,
                Country = a.Country,
                Books = new List<BookResponseDto>()
            }).ToList();

            var paginatedAuthors = new PaginatedResultDto<AuthorResponseDto>(authorDtos, authorDtos.Count, 1, 1);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllAuthorsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginatedAuthors);

            // Act
            var result = await _controller.GetAllAuthors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(paginatedAuthors, okResult.Value);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsOkResult_WithAuthor()
        {
            // Arrange
            var authors = TestDataSeeder.GetAuthors();
            var author = authors.FirstOrDefault(a => a.Id == 1);

            var authorDto = new AuthorResponseDto
            {
                Id = author.Id,
                AuthorFullName = $"{author.Name} {author.Surname}",
                DateOfBirth = author.DateOfBirth,
                Country = author.Country,
                Books = new List<BookResponseDto>()
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAuthorByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(authorDto);

            // Act
            var result = await _controller.GetAuthorById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(authorDto, okResult.Value);
        }

      
        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAtAction_WithCreatedAuthor()
        {
            // Arrange
            var newAuthor = new AuthorResponseDto { Id = 4, AuthorFullName = "New Author", DateOfBirth = DateTime.Now.AddYears(-30), Country = "Country" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newAuthor);

            // Act
            var result = await _controller.CreateAuthor(new AuthorRequestDto { AuthorFullName = "New Author", DateOfBirth = DateTime.Now.AddYears(-30), Country = "Country" });

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(newAuthor, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsOkResult_WithUpdatedAuthor()
        {
            // Arrange
            var updatedAuthor = new AuthorResponseDto { Id = 1, AuthorFullName = "Updated Author", DateOfBirth = DateTime.Now.AddYears(-40), Country = "USA" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateAuthorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedAuthor);

            // Act
            var result = await _controller.UpdateAuthor(1, new AuthorRequestDto { AuthorFullName = "Updated Author", DateOfBirth = DateTime.Now.AddYears(-40), Country = "USA" });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(updatedAuthor, okResult.Value);
        }
                
    }
}
