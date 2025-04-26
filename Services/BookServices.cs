using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public interface IBookServices
    {
        Task<IEnumerable<ReturnBookDto>> GetAllBooksAsync();
        Task<BookDto> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task UpdateBookAsync(int id, BookDto book);
        Task<bool> DeleteBookAsync(int id);
    }

    public class BookServices : IBookServices
    {
        private readonly AppDbContext context;

        public BookServices(AppDbContext context) {
            this.context = context;
        }


        public async Task<Book> AddBookAsync(Book book) {
            context.Books.Add(book);
            await context.SaveChangesAsync();
            return await context.Books
        .Include(b => b.Author)
        .Include(b => b.Category)
        .FirstOrDefaultAsync(b => b.BookId == book.BookId); 
        }

        public async Task<bool> DeleteBookAsync(int id) {
            var book = context.Books.Find(id);
            if (book == null)
            {
                return false;
            }
            context.Books.Remove(book);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<BookDto> GetBookByIdAsync(int id) {
            var book = await context.Books
                .Include(a => a.Author)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return null;
            }
            return new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                IsAvailable = book.IsAvailable,
                AuthorName = book.Author?.Name,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                CategoryName = book.Category?.Name

            };
        }

        public async Task<IEnumerable<ReturnBookDto>> GetAllBooksAsync() {
            var books = await context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();

            return books.Select(b => new ReturnBookDto
            {
                BookId = b.BookId,
                Title = b.Title,
                IsAvailable = b.IsAvailable,
                AuthorName = b.Author?.Name,
                CategoryName = b.Category?.Name
            });
        }

        public async Task UpdateBookAsync(int id, BookDto book) {
            var existingBook = await context.Books.FindAsync(id);

            if (existingBook == null)
                throw new KeyNotFoundException($"Book with id = {id} not found.");

            existingBook.Title = book.Title;
            existingBook.AuthorId = book.AuthorId;
            existingBook.CategoryId = book.CategoryId;

            context.Books.Update(existingBook);
            await context.SaveChangesAsync();
        }



    }
}
