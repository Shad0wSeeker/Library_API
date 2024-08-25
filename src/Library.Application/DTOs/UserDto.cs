using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // transmitted only when creating or changing a pass
        //public string ConfirmPassword { get; set; } // transmitted only when creating or changing a pass
        public string Role { get; set; }
    }
}
