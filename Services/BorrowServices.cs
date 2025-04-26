using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public interface IBorrowServices
    {
        Task<BorrowDto> BorrowBook(int bookId, int userId);
        Task ReturnBook(int borrowId);
        Task<IEnumerable<BorrowDto>> GetBorrowedBooksByUser(int userId);
    }
    public class BorrowServices : IBorrowServices
    {
        private readonly AppDbContext context;
        public BorrowServices(AppDbContext context) {
            this.context = context;
        }
        public async Task<BorrowDto> BorrowBook(int bookId, int userId) {
            var book = await context.Books.FindAsync(bookId);
            var user = await context.Users.FindAsync(userId);
                if (book == null || !book.IsAvailable)
            {
                return null;
            }
            var borrow = new Borrow
            {
                BookId = bookId,
                UserId = userId,
                IsReturned = false
            };
            context.Borrows.Add(borrow);
            book.IsAvailable = false;
            await context.SaveChangesAsync();
            return new BorrowDto
            {
                BorrowId = borrow.BorrowId,
                BookId = bookId,
                UserId = userId,
                IsReturned = false
            };
        }
        public async Task ReturnBook(int borrowId) {
            var borrow = await context.Borrows.FindAsync(borrowId);
            if (borrow == null)
            {
                return;
            }

            var book = await context.Books.FindAsync(borrow.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
            }
            borrow.IsReturned = true;
            context.Borrows.Remove(borrow);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<BorrowDto>> GetBorrowedBooksByUser(int userId) {
            var borrows = await context.Borrows
                .Where(b => b.UserId == userId && !b.IsReturned)
                .ToListAsync();
            return borrows.Select(b => new BorrowDto
            {
                BookId = b.BookId,
                UserId = b.UserId,
                IsReturned = b.IsReturned
            });
        }
    }
}
