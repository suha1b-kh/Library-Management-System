using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices service;

        public AuthController(IAuthServices service) {
            this.service = service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto userDto) {

            if (userDto == null)
                return BadRequest("User data is required");

            if (string.IsNullOrWhiteSpace(userDto.Username))
                return BadRequest("Username is required");

            if (string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("Password is required");

            if (string.IsNullOrWhiteSpace(userDto.Role))
                return BadRequest("Role is required");

            if (await service.UserExists(userDto.Username))
                return BadRequest("Username already exists");
            if (userDto.Password.Length < 8)
                return BadRequest("Password must be at least 8 characters long");




                var user = new User
            {
                UserName = userDto.Username,
                Password = userDto.Password,
                Role = userDto.Role,
            };
            var newUser = await service.Register(user);
            if (newUser == null)
                return BadRequest("Error While Sign up");

            var token = service.GenerateToken(newUser);
            return Ok(
                       new
                       {
                           Token = token,
                           UserId = user.UserId,
                           Username = user.UserName
                       });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto) {

            if (loginDto == null)
                return BadRequest("Login data is required");

            if (string.IsNullOrWhiteSpace(loginDto.Username))
                return BadRequest("Username is required");

            if (string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Password is required");

                var user = await service.Login(loginDto.Username, loginDto.Password);

            if (user == null)
                return Unauthorized("Username or password is incorrect");

            var token = service.GenerateToken(user);

            return Ok(
            new {
                Token = token,
                UserId = user.UserId,
                Username = user.UserName
            });
        }

    }
}
