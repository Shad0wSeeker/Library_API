﻿using Library.Domain.Models;
using Library.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository: IRepository<Book>
    {
        Task<Book> GetByIdAsync(int bookId, CancellationToken cancellationToken = default);
        Task<Book> GetByISBNAsync(string ISBN, CancellationToken cancellationToken = default);

    }
}
