using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{

    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
    }
    public class CategoryServices : ICategoryServices
    {
        private readonly AppDbContext context;
        public CategoryServices(AppDbContext context) {
            this.context = context;
        }
      
        public async Task<IEnumerable<CategoryDto>> GetAllCategories() {
            var categories = await context.Categories
        .Include(c => c.Books)
            .ThenInclude(b => b.Author)
        .ToListAsync();

            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.CategoryId,
                Name = c.Name,
                Books = c.Books.Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    IsAvailable = b.IsAvailable,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author?.Name,
                    CategoryId = b.CategoryId,
                    CategoryName = c.Name
                }).ToList()
            });

            return categoryDtos;
        }
        public async Task<CategoryDto> GetCategoryById(int id) {
            var category = await context.Categories
    .Include(c => c.Books)
        .ThenInclude(b => b.Author)
    .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = category.CategoryId,
                Name = category.Name,
                Books = category.Books.Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    IsAvailable = b.IsAvailable,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author?.Name,
                    CategoryId = b.CategoryId,
                    CategoryName = b.Category?.Name
                }).ToList()
            };
        }
        public async Task AddCategory(Category category) {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }
        public async Task UpdateCategory(Category category) {
            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }
        public async Task DeleteCategory(int id) {
            var category = await GetCategoryById(id);
            var newCategory = new Category
            {
                Name = category.Name,
                Books = (ICollection<Book>)category.Books,
                CategoryId = category.Id,
            };
            if (category != null)
            {
                context.Categories.Remove(newCategory);
                await context.SaveChangesAsync();
            }
        }
    }
}
