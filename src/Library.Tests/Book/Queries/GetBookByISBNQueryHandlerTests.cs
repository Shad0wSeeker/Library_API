using AutoMapper;
using Library.Application.Book.Queries.GetBookByISBN;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Domain.Interfaces;
using Library.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Book.Queries
{
    public class GetBookByISBNQueryHandlerTests
    {
        private readonly GetBookByISBNQueryHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public GetBookByISBNQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new GetBookByISBNQueryHandler(_mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenBookNotFound()
        {
            // Arrange
            var query = new GetBookByISBNQuery("9999999999");
            _unitOfWorkMock.Setup(u => u.Books.GetByISBNAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Library.Domain.Models.Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnBookResponseDto_WhenBookExists()
        {
            // Arrange
            var book = TestDataSeeder.GetBooks().Find(b => b.ISBN == "1234567890");
            var expectedBookResponseDto = new BookResponseDto
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Name = book.Name,
                Genre = book.Genre,
                Description = book.Description,
                AuthorId = book.AuthorId,
                ImagePath = book.ImagePath
            };

            _unitOfWorkMock.Setup(u => u.Books.GetByISBNAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(new GetBookByISBNQuery("1234567890"), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBookResponseDto.Id, result.Id);
            Assert.Equal(expectedBookResponseDto.ISBN, result.ISBN);
            Assert.Equal(expectedBookResponseDto.Name, result.Name);
            Assert.Equal(expectedBookResponseDto.Genre, result.Genre);
            Assert.Equal(expectedBookResponseDto.Description, result.Description);
            Assert.Equal(expectedBookResponseDto.AuthorId, result.AuthorId);
            Assert.Equal(expectedBookResponseDto.ImagePath, result.ImagePath);
        }
    }
}
