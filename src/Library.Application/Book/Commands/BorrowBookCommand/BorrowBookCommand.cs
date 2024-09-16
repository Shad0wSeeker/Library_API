using Library.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Book.Commands.BorrowBookCommand
{
    public class BorrowBookCommand : IRequest<BorrowBookDto>
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime BorrowingTime { get; set; }
        public DateTime ReturningTime { get; set; }

        public BorrowBookCommand(int bookId, int userId, DateTime borrowingTime, DateTime returningTime)
        {
            BookId = bookId;
            UserId = userId;
            BorrowingTime = borrowingTime;
            ReturningTime = returningTime;
        }
    }
}
