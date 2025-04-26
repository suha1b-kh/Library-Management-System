using Library_Management_System.Models;
using Library_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        ICategoryServices categoryServices;

        public CategoriesController(ICategoryServices categoryServices) {
            this.categoryServices = categoryServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories() {
            var categories = await categoryServices.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id) {
            var category =await categoryServices.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] string category) {
            if (string.IsNullOrEmpty(category))
            {
                return BadRequest("Category cannot be null or empty");
            }
            var newCategory = new Category
            {
                Name = category
            };
            categoryServices.AddCategory(newCategory);
            return CreatedAtAction(nameof(GetCategory), new { id = newCategory.CategoryId }, newCategory);
        }
    }
}
