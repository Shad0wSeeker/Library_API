using Library.Domain.Models;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Repositories
{
    public class BookRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly BookRepository _bookRepository;

        public BookRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _bookRepository = new BookRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var author = new Author
            {
                Id = 1,
                Name = "Author1",
                Surname = "Surname1",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Country = "Country1"
            };
            _context.Authors.Add(author);

            var book1 = new Book
            {
                Id = 1,
                ISBN = "1234567890",
                Name = "Book1",
                Genre = "Genre1",
                Description = "Description1",
                AuthorId = author.Id,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(10),
                ImagePath = "Path1"
            };
            var book2 = new Book
            {
                Id = 2,
                ISBN = "0987654321",
                Name = "Book2",
                Genre = "Genre2",
                Description = "Description2",
                AuthorId = author.Id,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(20),
                ImagePath = "Path2"
            };

            _context.Books.AddRange(book1, book2);
            _context.SaveChanges();
        }


        [Fact]
        public async Task AddAsync_AddsBookToDatabase()
        {
            // Arrange
            var newBook = new Book
            {
                ISBN = "1122334455",
                Name = "New Book",
                Genre = "New Genre",
                Description = "New Description",
                AuthorId = 1,
                BorrowingTime = DateTime.Now,
                ReturningTime = DateTime.Now.AddDays(15),
                ImagePath = "NewPath"
            };

            // Act
            var addedBook = await _bookRepository.AddAsync(newBook);

            // Assert
            Assert.NotNull(addedBook);
            Assert.Equal(newBook.ISBN, addedBook.ISBN);
            Assert.Equal(newBook.Name, addedBook.Name);
            var bookInDb = await _context.Books.FindAsync(addedBook.Id);
            Assert.NotNull(bookInDb);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBook_WhenBookExists()
        {
            // Act
            var book = await _bookRepository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(book);
            Assert.Equal(1, book.Id);
            Assert.Equal("Book1", book.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            // Act
            var book = await _bookRepository.GetByIdAsync(999);

            // Assert
            Assert.Null(book);
        }

        [Fact]
        public async Task GetByISBNAsync_ReturnsBook_WhenBookExists()
        {
            // Act
            var book = await _bookRepository.GetByISBNAsync("1234567890");

            // Assert
            Assert.NotNull(book);
            Assert.Equal("Book1", book.Name);
        }

        [Fact]
        public async Task GetByISBNAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            // Act
            var book = await _bookRepository.GetByISBNAsync("0000000000");

            // Assert
            Assert.Null(book);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPaginatedBooks()
        {
            // Act
            var result = await _bookRepository.GetAllAsync(1, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task DeleteAsync_RemovesBookFromDatabase()
        {
            // Act
            await _bookRepository.DeleteAsync(1);
            var book = await _context.Books.FindAsync(1);

            // Assert
            Assert.Null(book);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
