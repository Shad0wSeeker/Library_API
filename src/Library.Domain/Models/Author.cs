using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }

        public ICollection<Book> Books { get; set; }

        public Author() { }
        public Author(string name, string surname, DateTime dateOfBirth, string country, ICollection<Book> books)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            Country = country;
        }
    }
}
