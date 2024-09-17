using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Application.User.Queries.GetUserById;
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
    public class GetUserByIdQueryHandlerTests
    {
        private readonly GetUserByIdQueryHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new GetUserByIdQueryHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var query = new GetUserByIdQuery(1);
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
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

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(new GetUserByIdQuery(user.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userResponseDto.Id, result.Id);
            Assert.Equal(userResponseDto.Email, result.Email);
            Assert.Equal(userResponseDto.Role, result.Role);
        }
    }
}
