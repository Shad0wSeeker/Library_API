using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Data
{
    public class DBInitializer
    {
        public static async Task SeedData(AppDbContext context)
        {

            var authors = new List<Author>
        {
            new Author
            {
                Name = "J.K.", 
                Surname = "Rowling",
                DateOfBirth = new DateTime(1965, 7, 31),
                Country = "United Kingdom"
            },

            new Author 
            { 
                Name = "George",
                Surname = "Orwell",
                DateOfBirth = new DateTime(1903, 6, 25), 
                Country = "United Kingdom" 
            },

            new Author
            {
                Name=" Dan",
                Surname=" Brown",
                DateOfBirth=new DateTime(1964, 6, 22),
                Country="USA"
            }
        };

            var books = new List<Book>
        {
            new Book
            {
                ISBN = "9780747532743",
                Name = "Harry Potter and the Philosopher's Stone",
                Genre = "Fantasy",
                Description = "A young wizard's journey begins.",
                Author = authors[0],
                BorrowingTime = DateTime.UtcNow,
                ReturningTime = DateTime.UtcNow.AddDays(14),
                ImagePath="local" 
            },

            new Book
            {
                ISBN = "9780451524935", Name = "1984",
                Genre = "Dystopian",
                Description = "A novel about a totalitarian regime.",
                Author = authors[1],
                BorrowingTime = DateTime.UtcNow,
                ReturningTime = DateTime.UtcNow.AddDays(14),
                ImagePath="local"    
            },

            new Book
            {
                ISBN = "123456789",
                Name="Angels & Demons",
                Genre="mystery - thriller",
                Description="This is a short article about the fascinating and tragic history of the oldest satanic brotherhood in the world.",
                Author = authors[2],
                BorrowingTime = DateTime.UtcNow,
                ReturningTime=DateTime.UtcNow.AddDays(10),
                ImagePath="local"
            }
        };

            var users = new List<User>
            {
                new User("admin@example.com",  "hashed_password_1", UserRole.Admin),

                new User("client@example.com", "hashed_password_2", UserRole.Client),

                new User("user2@example.com", "pass2", UserRole.Client)
            };


            var newAuthors = authors
                .Where(author => !context.Authors
                    .Any(a => a.Name == author.Name && a.Surname == author.Surname && a.DateOfBirth == author.DateOfBirth))
                .ToList();

            if (newAuthors.Any())
            {
                context.Authors.AddRange(newAuthors);
                await context.SaveChangesAsync();
            }

            var newBooks = books
            .Where(book => !context.Books
                .Any(b => b.ISBN == book.ISBN))
            .ToList();

            if (newBooks.Any())
            {
                context.Books.AddRange(newBooks);
                await context.SaveChangesAsync();
            }


            var newUsers = users
            .Where(user => !context.Users
                .Any(u => u.Email == user.Email))
            .ToList();

            if (newUsers.Any())
            {
                context.Users.AddRange(newUsers);
                await context.SaveChangesAsync();
            }


        }
    }
}
