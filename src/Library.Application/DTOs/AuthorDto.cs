using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Library.Application.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; } 
        public string AuthorFullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }

        public List<BookDto> Books { get; set; } = new List<BookDto>();
    }

   
}
