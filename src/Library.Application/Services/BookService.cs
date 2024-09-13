using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResultDto<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var paginatedBooks = await _unitOfWork.Books.GetAllAsync(
                pageNumber,
                pageSize,
                query => query.Include(a => a.Author),
                cancellationToken
            );

            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(paginatedBooks.Items);

            return new PaginatedResultDto<BookDto>(bookDtos, paginatedBooks.TotalCount, pageSize, pageNumber);
        }

        public async Task<BookDto> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken = default)
        {
            var book = await _unitOfWork.Books.GetByISBNAsync(isbn, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> CreateBookAsync(BookDto BookDto, CancellationToken cancellationToken = default)
        {
            var book = _mapper.Map<Book>(BookDto);
            await _unitOfWork.Books.AddAsync(book, cancellationToken);
            
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(int id, BookDto bookDto, CancellationToken cancellationToken = default)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id, cancellationToken);

            if (book == null)
            {
                return null; 
            }

            _mapper.Map(bookDto, book);

            await _unitOfWork.Books.UpdateAsync(book, cancellationToken);
            

            return _mapper.Map<BookDto>(book);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken = default)
        {
           await _unitOfWork.Books.DeleteAsync(id, cancellationToken);
            
        }


        public async Task<BorrowBookDto> BorrowBookAsync(BorrowBookDto borrowBookDto, CancellationToken cancellationToken = default)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(borrowBookDto.BookId, cancellationToken);
            var user = await _unitOfWork.Users.GetByIdAsync(borrowBookDto.UserId, cancellationToken);

            if (book == null || user == null)
            {
                return null;
            }
            if (book.BorrowingTime < book.ReturningTime)
            {
                throw new InvalidOperationException("The book is already borrowed by another user.");
            }

            user.BorrowedBooks ??= new List<Book>();
            user.BorrowedBooks.Add(book);

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            

            var borrowedBookDto = new BorrowBookDto
            {
                BookId = book.Id,
                UserId = user.Id,
                BorrowingTime = DateTime.Now, 
                ReturningTime = DateTime.Now.AddDays(14) 
            };

            return borrowedBookDto;
        }



    }
}
