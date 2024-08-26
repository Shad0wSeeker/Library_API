using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Library.Shared.DTO;
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
        public async Task<PaginatedResultDto<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize)
        {
            var paginatedBooks = await _unitOfWork.Books.GetAllAsync(pageNumber, pageSize);
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(paginatedBooks.Items);

            return new PaginatedResultDto<BookDto>(bookDtos, paginatedBooks.TotalCount, pageSize, pageNumber);
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> GetBookByISBNAsync(string isbn)
        {
            var book = await _unitOfWork.Books.GetByISBNAsync(isbn);
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> CreateBookAsync(BookDto BookDto)
        {
            var book = _mapper.Map<Book>(BookDto);
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(int id, BookDto bookDto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
            {
                return null; // Книга не найдена
            }

            // Обновляем данные книги с помощью AutoMapper
            _mapper.Map(bookDto, book);

            // Обновляем книгу в базе данных
            await _unitOfWork.Books.UpdateAsync(book);
            await _unitOfWork.CompleteAsync();

            // Возвращаем обновленные данные книги
            return _mapper.Map<BookDto>(book);
        }

        public async Task DeleteBookAsync(int id)
        {
           await _unitOfWork.Books.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<BorrowBookDto> BorrowBookAsync(BorrowBookDto borrowBookDto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(borrowBookDto.BookId);
            var user = await _unitOfWork.Users.GetByIdAsync(borrowBookDto.UserId);

            if (book == null || user == null)
            {
                return null;
            }

            user.BorrowedBooks ??= new List<Book>();
            user.BorrowedBooks.Add(book);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

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
