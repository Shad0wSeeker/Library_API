using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Infrastructure.Data;
using Library.Domain.Models;

namespace Library.Tests.Repositories
{
    public class AuthorRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AuthorRepository _repository;

        public AuthorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new AuthorRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Удаляем все существующие записи из базы данных
            _context.Authors.RemoveRange(_context.Authors);
            _context.SaveChanges();

            // Добавляем новые записи
            _context.Authors.AddRange(
                new Author
                {
                    Id = 1, // Указываем ID для тестирования
                    Name = "John",
                    Surname = "Doe",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Country = "USA",
                    Books = new List<Book>() // Пустая коллекция, т.к. это поле не инициализируется в конструкторе
                },
                new Author
                {
                    Id = 2,
                    Name = "Jane",
                    Surname = "Smith",
                    DateOfBirth = new DateTime(1990, 2, 15),
                    Country = "UK",
                    Books = new List<Book>()
                }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenAuthorDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999); // Не существующий ID

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPaginatedAuthors()
        {
            // Act
            var result = await _repository.GetAllAsync(1, 10); // Страница 1, размер 10

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count()); // Ожидаем 2 автора в базе данных
        }

        [Fact]
        public async Task DeleteAsync_RemovesAuthorFromDatabase()
        {
            // Arrange
            var author = new Author
            {
                Name = "Mark",
                Surname = "Twain",
                DateOfBirth = new DateTime(1835, 11, 30),
                Country = "USA",
                Books = new List<Book>()
            };
            await _repository.AddAsync(author);

            // Act
            await _repository.DeleteAsync(author.Id);
            var result = await _repository.GetByIdAsync(author.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsAuthorToDatabase()
        {
            var author = new Author
            {
                Name = "Ernest",
                Surname = "Hemingway",
                DateOfBirth = new DateTime(1899, 7, 21),
                Country = "USA",
                Books = new List<Book>()
            };

            await _repository.AddAsync(author);

            var addedAuthorId = author.Id;
            Assert.NotEqual(0, addedAuthorId);

            var result = await _repository.GetByIdAsync(addedAuthorId);
            Assert.NotNull(result);
            Assert.Equal(author.Name, result.Name);
            Assert.Equal(author.Surname, result.Surname);
            Assert.Equal(author.DateOfBirth, result.DateOfBirth);
            Assert.Equal(author.Country, result.Country);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAuthorInDatabase()
        {
            var author = new Author
            {
                Name = "John",
                Surname = "Doe",
                DateOfBirth = new DateTime(1970, 1, 1),
                Country = "USA"
            };

            var addedAuthor = await _repository.AddAsync(author);

            addedAuthor.Name = "John Updated";
            addedAuthor.Surname = "Doe Updated";
            await _repository.UpdateAsync(addedAuthor);

            var result = await _repository.GetByIdAsync(addedAuthor.Id);

            Assert.NotNull(result);
            Assert.Equal("John Updated", result.Name);
            Assert.Equal("Doe Updated", result.Surname);
        }


        
        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

