using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Application.User.Queries.AuthenticateUser;
using Library.Domain.Interfaces;
using Library.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.User.Queries
{
    public class AuthenticateUserQueryHandlerTests
    {
        private readonly AuthenticateUserQueryHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public AuthenticateUserQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new AuthenticateUserQueryHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var query = new AuthenticateUserQuery("nonexistent@example.com");
            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Library.Domain.Models.User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnUserResponseDto_WhenUserExists()
        {
            // Arrange
            var user = TestDataSeeder.GetUsers()[0];

            var userResponseDto = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            _unitOfWorkMock.Setup(u => u.Users.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(new AuthenticateUserQuery(user.Email), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userResponseDto.Id, result.Id);
            Assert.Equal(userResponseDto.Email, result.Email);
            Assert.Equal(userResponseDto.Role, result.Role);
        }
    }
}
