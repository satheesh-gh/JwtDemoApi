using JwtDemoApi.Data;
using JwtDemoApi.DTOs;
using JwtDemoApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtDemoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<ApiUser> _passwordHasher;

        public AuthController(
            AdventureWorksDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<ApiUser>();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            //var hashedpassword = _passwordHasher.HashPassword(null, request.Password);

            var user = _context.ApiUsers
                .FirstOrDefault(x =>
                    x.Username == request.Username &&
                    x.IsActive);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }



            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateToken(user);

            return Ok(new
            {
                token
            });
        }

        private string GenerateToken(ApiUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new[]
            {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Username),

            new Claim(
                ClaimTypes.Role,
                user.Role)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(
                        jwtSettings["ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var username = User.Identity?.Name;

            var role = User.FindFirst(
                System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Username = username,
                Role = role
            });
        }
    }
}
