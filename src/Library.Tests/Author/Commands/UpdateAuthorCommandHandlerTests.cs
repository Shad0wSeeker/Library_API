using AutoMapper;
using Library.Application.Author.Commands.UpdateAuthorCommand;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Author.Commands
{
    public class UpdateAuthorCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;
        private readonly UpdateAuthorCommandHandler _handler;

        public UpdateAuthorCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new UpdateAuthorCommandHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var updateRequest = new AuthorRequestDto
            {
                AuthorFullName = "Updated Name",
                DateOfBirth = DateTime.Now.AddYears(-25),
                Country = "Updated Country"
            };

            _mockUnitOfWork.Setup(uow => uow.Authors.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Library.Domain.Models.Author)null);

            var command = new UpdateAuthorCommand(1, updateRequest);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAuthorFullNameIsInvalid()
        {
            // Arrange
            var author = new Library.Domain.Models.Author { Id = 1, Name = "John", Surname = "Doe", DateOfBirth = DateTime.Now.AddYears(-30), Country = "USA" };
            var updateRequest = new AuthorRequestDto
            {
                AuthorFullName = "InvalidName", // No space
                DateOfBirth = DateTime.Now.AddYears(-25),
                Country = "Updated Country"
            };

            _mockUnitOfWork.Setup(uow => uow.Authors.GetByIdAsync(author.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            var command = new UpdateAuthorCommand(author.Id, updateRequest);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
