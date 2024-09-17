using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Data
{
    public static class TestDataSeeder
    {
        public static List<Library.Domain.Models.Author> GetAuthors()
        {
            return new List<Library.Domain.Models.Author>
            {
                new Library.Domain.Models.Author
                {
                    Id = 1,
                    Name = "John",
                    Surname = "Doe",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Country = "USA",
                    Books = new List<Library.Domain.Models.Book>()
                },
                new Library.Domain.Models.Author
                {
                    Id = 2,
                    Name = "Jane",
                    Surname = "Smith",
                    DateOfBirth = new DateTime(1990, 2, 15),
                    Country = "UK",
                    Books = new List<Library.Domain.Models.Book>()
                },
                new Library.Domain.Models.Author
                {
                    Id = 3,
                    Name = "Author1",
                    Surname = "Surname1",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Country = "Country1"
                }
            };
        }

        public static List<Library.Domain.Models.Book> GetBooks()
        {
            return new List<Library.Domain.Models.Book>
            {
                new Library.Domain.Models.Book
                {
                    Id = 1,
                    ISBN = "1234567890",
                    Name = "Book1",
                    Genre = "Genre1",
                    Description = "Description1",
                    AuthorId = 3,
                    BorrowingTime = DateTime.Now,
                    ReturningTime = DateTime.Now.AddDays(10),
                    ImagePath = "Path1"
                },
                new Library.Domain.Models.Book
                {
                    Id = 2,
                    ISBN = "0987654321",
                    Name = "Book2",
                    Genre = "Genre2",
                    Description = "Description2",
                    AuthorId = 2,
                    BorrowingTime = DateTime.Now,
                    ReturningTime = DateTime.Now.AddDays(20),
                    ImagePath = "Path2"
                }
            };
        }

        public static List<Library.Domain.Models.User> GetUsers()
        {
            return new List<Library.Domain.Models.User>
            {
                new Library.Domain.Models.User { Id = 1, Email = "test@example.com", Password = "password123" },
                new Library.Domain.Models.User { Id = 2, Email = "user@example.com", Password = "password456" }
            };
        }
    }
}
