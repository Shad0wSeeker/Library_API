﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.DTOs
{
    public class BookRequestDto
    {
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public DateTime BorrowingTime { get; set; }
        public DateTime ReturningTime { get; set; }
        public string ImagePath { get; set; }
    }

    public class BookResponseDto
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string ImagePath { get; set; }
    }
}
