using AutoMapper;
using Library.Application.Author.Queries.GetAllAuthors;
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

namespace Library.Tests.Author.Queries
{
    public class GetAllAuthorsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;
        private readonly GetAllAuthorsQueryHandler _handler;

        public GetAllAuthorsQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
            _handler = new GetAllAuthorsQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaginatedResult_WhenAuthorsExist()
        {
            // Arrange
            var authors = TestDataSeeder.GetAuthors();
            var paginatedResult = new PaginatedResultDto<Library.Domain.Models.Author>(authors, authors.Count, 10, 1);

            _mockUnitOfWork.Setup(uow => uow.Authors.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Library.Domain.Models.Author>, IQueryable<Library.Domain.Models.Author>>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(paginatedResult);

            var query = new GetAllAuthorsQuery(1, 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(3, result.Items.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResult_WhenNoAuthorsExist()
        {
            // Arrange
            var paginatedResult = new PaginatedResultDto<Library.Domain.Models.Author>(
                Enumerable.Empty<Library.Domain.Models.Author>(),0,10, 1 );

            _mockUnitOfWork.Setup(uow => uow.Authors.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Library.Domain.Models.Author>, IQueryable<Library.Domain.Models.Author>>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(paginatedResult);

            var query = new GetAllAuthorsQuery(1, 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(0, result.TotalPages);
            Assert.Empty(result.Items);
        }
    }
}
