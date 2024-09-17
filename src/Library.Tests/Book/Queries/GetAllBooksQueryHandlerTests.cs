using AutoMapper;
using Library.Application.Book.Queries.GetAllBooks;
using Library.Application.DTOs;
using Library.Application.Mapper;
using Library.Domain.Interfaces;
using Library.Shared.DTO;
using Library.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Book.Queries
{
    public class GetAllBooksQueryHandlerTests
    {
        private readonly GetAllBooksQueryHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public GetAllBooksQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Library.Domain.Models.Book, BookResponseDto>();
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new GetAllBooksQueryHandler(_mapper, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedResultDto_WhenBooksExist()
        {
            // Arrange
            var books = TestDataSeeder.GetBooks();
            var query = new GetAllBooksQuery(1, 10); 

            var paginatedBooks = new PaginatedResultDto<Library.Domain.Models.Book>(
                books,
                books.Count,
                query.PageSize,
                query.PageNumber
            );

            _unitOfWorkMock.Setup(u => u.Books.GetAllAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Func<IQueryable<Library.Domain.Models.Book>, IQueryable<Library.Domain.Models.Book>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(paginatedBooks);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(books.Count, result.TotalCount);
            Assert.Equal(query.PageSize, result.PageSize);
            Assert.Equal(query.PageNumber, result.CurrentPage);
            Assert.Equal(books.Count, result.Items.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResult_WhenNoBooksExist()
        {
            // Arrange
            var query = new GetAllBooksQuery(1, 10);

            var paginatedBooks = new PaginatedResultDto<Library.Domain.Models.Book>(
                Enumerable.Empty<Library.Domain.Models.Book>(),
                0,
                query.PageSize,
                query.PageNumber
            );

            _unitOfWorkMock.Setup(u => u.Books.GetAllAsync(
                query.PageNumber,
                query.PageSize,
                It.IsAny<Func<IQueryable<Library.Domain.Models.Book>, IQueryable<Library.Domain.Models.Book>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(paginatedBooks);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(query.PageSize, result.PageSize);
            Assert.Equal(query.PageNumber, result.CurrentPage);
            Assert.Empty(result.Items);
        }
    }
}
