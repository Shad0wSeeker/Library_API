using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models
{
    public enum UserRole
    {
        Admin,
        Client
    }
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserRole Role { get; set; }
        public ICollection<Book>? BorrowedBooks { get; set; }


        public User() { }

        public User(string email, string password, UserRole role)
        {
            Email = email;
            Password = password;
            Role = role;
        }




    }
}
