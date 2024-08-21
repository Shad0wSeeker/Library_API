using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class BorrowBookDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime BorrowingTime { get; set; }
        public DateTime ReturningTime { get; set; }
    }
}
