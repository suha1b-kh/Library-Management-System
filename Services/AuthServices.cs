using Library_Management_System.Dtos;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Management_System.Services
{
    public interface IAuthServices
    {
        Task<User> Register(User user);
        Task<User> Login(string username, string password);
        string GenerateToken(User user);
        Task<bool> UserExists(string name);
    }
    public class AuthServices : IAuthServices
    {
        private readonly AppDbContext context;
        private readonly IConfiguration config;

        public AuthServices(AppDbContext context, IConfiguration config) {
            this.context = context;
            this.config = config;
        }

        public string GenerateToken(User user) {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<User> Login(string username, string password) {
            var user = await context.Users
            .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);

            return user;
        }

        public async Task<User> Register(User user) {
            
            if (await context.Users.AnyAsync(u => u.UserName == user.UserName))
            {
                return null;
            }

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public async Task<bool> UserExists(string name) {
            return await context.Users.AnyAsync(u => u.UserName.ToLower() == name.ToLower());
        }
    }
}
