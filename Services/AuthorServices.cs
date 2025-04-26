using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{

    public interface IAuthorServices
    {
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<Author> AddAuthor(Author author);
        Task<Author> UpdateAuthor(int id, AuthorDto author);
        Task<bool> DeleteAuthor(int id);
        Task<Author> FindAuthor(int id);
    }
    public class AuthorServices : IAuthorServices
    {
        private readonly AppDbContext context;

        public AuthorServices(AppDbContext context) {
            this.context = context;
        }

        public async Task<Author> AddAuthor(Author author) {
            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();
            return (author);
        }

        public async Task<bool> DeleteAuthor(int id) {
            var author = await context.Authors.FindAsync(id);
            if(author == null)
            {
                return false;
            }
            context.Authors.Remove(author);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Author> FindAuthor(int id) {
            var author = await context.Authors.FindAsync(id);
            if (author == null)
            {
                return null;
            }
            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors() {
            var authors = await context.Authors.Include(a => a.Books).ToListAsync();
            return authors;
        }

        public async Task<Author> UpdateAuthor(int id, AuthorDto author) {
            var existingAuthor = await context.Authors.FindAsync(id);

            if (existingAuthor == null)
                return null;

            existingAuthor.Name = author.Name;

            context.Authors.Update(existingAuthor);
            await context.SaveChangesAsync();
            return existingAuthor;
        }
    }
}
