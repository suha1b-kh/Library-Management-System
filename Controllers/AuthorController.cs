using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorServices services;

        public AuthorController(IAuthorServices service) {
            this.services = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors() {
            try
            {
                var authors = await services.GetAllAuthors();
                if (authors == null)
                    return NotFound("No authors found");

                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorDto authorDto) {
            try
            {
                if (authorDto == null)
                    return BadRequest("Author data is required");

                if (string.IsNullOrWhiteSpace(authorDto.Name))
                    return BadRequest("Author name is required");

                var author = new Author
                {
                    Name = authorDto.Name
                };

                await services.AddAuthor(author);
                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDto authorDto) {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid ID");


                var updatedAuthor = await services.UpdateAuthor(id, authorDto);
                if (updatedAuthor == null)
                    return NotFound($"Author with id = {id} not found");

                return Ok(updatedAuthor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int id) {
            try
            {
                if (id <= 0)
                    return BadRequest("Valid author ID is required");

                var req = await services.DeleteAuthor(id);
                if (req)
                    return Ok($"Author with id = {id} deleted successfully");
                else
                    return NotFound($"Author with id = {id} not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
