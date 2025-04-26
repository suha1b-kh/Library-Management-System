using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices bookServices;

        public BooksController(IBookServices bookServices) {
            this.bookServices = bookServices;
        }


            [HttpGet]
            public async Task<IActionResult> GetAllBooksAsync() {
                try
                {
                    var books = await bookServices.GetAllBooksAsync();
                    if (books == null)
                    {
                        return NotFound("No books found.");
                    }
                    return Ok(books);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetBook(int id) {
                if (id <= 0)
                    return BadRequest("Invalid ID.");

                try
                {
                    var book = await bookServices.GetBookByIdAsync(id);
                    if (book == null)
                    {
                        return NotFound($"Book with id = {id} not found.");
                    }
                    return Ok(book);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> AddBook([FromBody] BookDto book) {
                if (book == null)
                    return BadRequest("Book cannot be null.");
                try
                {
                    var newBook = new Book
                    {
                        Title = book.Title,
                        AuthorId = book.AuthorId,
                        CategoryId = book.CategoryId,
                        IsAvailable = true
                    };

                    var addedBook  = await bookServices.AddBookAsync(newBook);

                    return CreatedAtAction(nameof(GetBook), new { id = addedBook.BookId }, addedBook);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpPut("{id}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto book) {
                if (id <= 0)
                    return BadRequest("Invalid ID.");

                if (book == null)
                    return BadRequest("Book data cannot be null.");

                try
                {
                    var existingBook = await bookServices.GetBookByIdAsync(id);
                    if (existingBook == null)
                        return NotFound($"Book with id = {id} not found.");
             

                    await bookServices.UpdateBookAsync(id, existingBook);

                    return Ok(existingBook);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            [HttpDelete("{id}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> DeleteBook(int id) {
                if (id <= 0)
                    return BadRequest("Invalid ID.");

                try
                {
                    var req = await bookServices.DeleteBookAsync(id);
                    if (req)
                        return Ok($"Book with id = {id} deleted successfully.");
                    else
                        return NotFound($"Book with id = {id} not found.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }



    }
