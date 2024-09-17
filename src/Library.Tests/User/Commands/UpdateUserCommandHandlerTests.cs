using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Application.User.Commands.UpdateUserCommand;
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
    public class UpdateUserCommandHandlerTests
    {
        private readonly UpdateUserCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new UpdateUserCommandHandler(_mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var command = new UpdateUserCommand(1, new UserRequestDto
            {
                Email = "newemail@example.com",
                Password = "newpassword",
                Role = "Admin"
            });
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Library.Domain.Models.User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
                
    }
}
