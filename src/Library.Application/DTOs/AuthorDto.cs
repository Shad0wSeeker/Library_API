using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Library.Application.DTOs
{    
    public class AuthorRequestDto
    {
        public string AuthorFullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
    }

    public class AuthorResponseDto
    {
        public int Id { get; set; }
        public string AuthorFullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public List<BookResponseDto> Books { get; set; } = new List<BookResponseDto>();
    }


}
