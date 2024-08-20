using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime BorrowingTime { get; set; }
        public DateTime ReturningTime { get; set; }
        public string ImagePath { get; set; }


        public Book() { }
        public Book(string ISBN, string Name, string Genre, string Description, int AuthorId, DateTime BorrowingTime, DateTime ReturningTime, string ImagePath)
        {
            this.ISBN = ISBN;
            this.Name = Name;
            this.Genre = Genre;
            this.Description = Description;
            this.AuthorId = AuthorId;
            this.ImagePath = ImagePath;
        }

    }
}
