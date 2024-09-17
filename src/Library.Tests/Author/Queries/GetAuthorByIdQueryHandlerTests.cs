using AutoMapper;
using Library.Application.Author.Queries.GetAuthorById;
using Library.Application.Mapper;
using Library.Domain.Interfaces;
using Library.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Author.Queries
{
    public class GetAuthorByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;
        private readonly GetAuthorByIdQueryHandler _handler;

        public GetAuthorByIdQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
            _handler = new GetAuthorByIdQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        
        [Fact]
        public async Task Handle_ShouldThrowException_WhenAuthorNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(uow => uow.Authors.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Library.Domain.Models.Author)null);

            var query = new GetAuthorByIdQuery(999);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
