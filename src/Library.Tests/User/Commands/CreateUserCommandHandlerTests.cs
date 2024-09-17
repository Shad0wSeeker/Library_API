using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Application.User.Commands.CreateUserCommand;
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
    public class CreateUserCommandHandlerTests
    {
        private readonly CreateUserCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public CreateUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile()); 
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new CreateUserCommandHandler(_mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenUserWithEmailExists()
        {
            // Arrange
            var existingUser = TestDataSeeder.GetUsers()[0];

            var command = new CreateUserCommand(new UserRequestDto
            {
                Email = existingUser.Email,
                Password = "newpassword",
                Role = "Client"
            });

            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
                
    }
}
