using Library.Domain.Interfaces;
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
    public class UserRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _userRepository = new UserRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Users.Add(new User { Id = 1, Email = "test@example.com", Password = "password123" });
            _context.Users.Add(new User { Id = 2, Email = "user@example.com", Password = "password456" });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;

            // Act
            var user = await _userRepository.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 999;

            // Act
            var user = await _userRepository.GetByIdAsync(userId);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task AddAsync_AddsUserToDatabase()
        {
            // Arrange
            var user = new User { Id = 3, Email = "newuser@example.com", Password = "newpassword" };

            // Act
            var addedUser = await _userRepository.AddAsync(user);
            await _context.SaveChangesAsync();
            var userFromDb = await _context.Users.FindAsync(user.Id);

            // Assert
            Assert.NotNull(userFromDb);
            Assert.Equal(user.Email, userFromDb.Email);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesUserInDatabase()
        {
            // Arrange
            var user = await _context.Users.FindAsync(1);
            user.Email = "updated@example.com";

            // Act
            var updatedUser = await _userRepository.UpdateAsync(user);
            await _context.SaveChangesAsync();
            var userFromDb = await _context.Users.FindAsync(user.Id);

            // Assert
            Assert.NotNull(userFromDb);
            Assert.Equal("updated@example.com", userFromDb.Email);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUserFromDatabase()
        {
            // Arrange
            var userId = 2;

            // Act
            await _userRepository.DeleteAsync(userId);
            await _context.SaveChangesAsync();
            var userFromDb = await _context.Users.FindAsync(userId);

            // Assert
            Assert.Null(userFromDb);
        }

        [Fact]
        public async Task GetByEmailAndPasswordAsync_ReturnsUser_WhenCredentialsMatch()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password123";

            // Act
            var user = await _userRepository.GetByEmailAndPasswordAsync(email, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public async Task GetByEmailAndPasswordAsync_ReturnsNull_WhenCredentialsDoNotMatch()
        {
            // Arrange
            var email = "wrong@example.com";
            var password = "wrongpassword";

            // Act
            var user = await _userRepository.GetByEmailAndPasswordAsync(email, password);

            // Assert
            Assert.Null(user);
        }
    }
}
