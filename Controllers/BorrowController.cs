using Library_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowServices borrowServices;
        public BorrowController(IBorrowServices borrowServices) {
            this.borrowServices = borrowServices;
        }
        [HttpPost("borrow Book")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BorrowBook(int bookId, int userId) {
            var result = await borrowServices.BorrowBook(bookId, userId);
            if (result == null)
            {
                return NotFound("Book not available");
            }
            return Ok(result);
        }
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(int borrowId) {
            await borrowServices.ReturnBook(borrowId);
            return Ok("Book Returned Successfully");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBorrowedBooksByUser(int userId) {
            var result = await borrowServices.GetBorrowedBooksByUser(userId);
            return Ok(result);
        }
    }
}
