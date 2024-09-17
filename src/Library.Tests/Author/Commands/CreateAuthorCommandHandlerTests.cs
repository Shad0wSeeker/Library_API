using AutoMapper;
using Library.Application.Author.Commands.CreateAuthorCommand;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Author.Commands
{
    public class CreateAuthorCommandHandlerTests
    {
        private readonly CreateAuthorCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public CreateAuthorCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new CreateAuthorCommandHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenAuthorFullNameIsInvalid()
        {
            // Arrange
            var command = new CreateAuthorCommand(new AuthorRequestDto { AuthorFullName = "John", DateOfBirth = DateTime.Now, Country = "USA" });

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnAuthorResponseDto_WhenAuthorIsValid()
        {
            // Arrange
            var authorRequest = new AuthorRequestDto
            {
                AuthorFullName = "John Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                Country = "USA"
            };

            var author = new Library.Domain.Models.Author
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                Country = "USA"
            };

            _unitOfWorkMock.Setup(u => u.Authors.AddAsync(It.IsAny<Library.Domain.Models.Author>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(author);

            var command = new CreateAuthorCommand(authorRequest);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.AuthorFullName);
            Assert.Equal(new DateTime(1980, 1, 1), result.DateOfBirth);
            Assert.Equal("USA", result.Country);
        }
    }
}
