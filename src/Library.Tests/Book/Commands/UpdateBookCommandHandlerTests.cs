using AutoMapper;
using Library.Application.Book.Commands.UpdateBookCommand;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Book.Commands
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly UpdateBookCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new UpdateBookCommandHandler(_mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Library.Domain.Models.Book)null);

            var command = new UpdateBookCommand(1, new BookRequestDto
            {
                ISBN = "1234567890",
                Name = "Updated Book",
                Genre = "Updated Genre",
                Description = "Updated Description",
                AuthorId = 1,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(10),
                ImagePath = "Updated Path"
            });

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
