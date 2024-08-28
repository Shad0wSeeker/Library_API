using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Shared.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _authorService = new AuthorService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_ReturnsPaginatedResult()
        {
            // Arrange
            var authors = new List<Author> { new Author { Id = 1, Name = "Author 1" } };
            var paginatedResultDto = new PaginatedResultDto<Author>(authors, authors.Count, 10, 1);
            _unitOfWorkMock.Setup(u => u.Authors.GetAllAsync(1, 10)).ReturnsAsync(paginatedResultDto);

            _mapperMock.Setup(m => m.Map<IEnumerable<AuthorDto>>(It.IsAny<IEnumerable<Author>>()))
                       .Returns(new List<AuthorDto> { new AuthorDto { Id = 1, AuthorFullName = "Author 1" } });

            // Act
            var result = await _authorService.GetAllAuthorsAsync(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ReturnsAuthorDto_WhenAuthorExists()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author 1" };
            _unitOfWorkMock.Setup(u => u.Authors.GetByIdAsync(1)).ReturnsAsync(author);
            _mapperMock.Setup(m => m.Map<AuthorDto>(It.IsAny<Author>())).Returns(new AuthorDto { Id = 1, AuthorFullName = "Author 1" });

            // Act
            var result = await _authorService.GetAuthorByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task CreateAuthorAsync_CreatesAuthor_ReturnsAuthorDto()
        {
            // Arrange
            var authorDto = new AuthorDto { Id = 1, AuthorFullName = "Author 1" };
            var author = new Author { Id = 1, Name = "Author 1" };

            _mapperMock.Setup(m => m.Map<Author>(authorDto)).Returns(author);
            _unitOfWorkMock.Setup(u => u.Authors.AddAsync(author)).ReturnsAsync(author);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<AuthorDto>(author)).Returns(authorDto);

            // Act
            var result = await _authorService.CreateAuthorAsync(authorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task DeleteAuthorAsync_CallsDeleteAsync_Once_WhenAuthorExists()
        {
            // Arrange
            var authorId = 1;
            _unitOfWorkMock.Setup(u => u.Authors.DeleteAsync(authorId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            await _authorService.DeleteAuthorAsync(authorId);

            // Assert
            _unitOfWorkMock.Verify(u => u.Authors.DeleteAsync(authorId), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
