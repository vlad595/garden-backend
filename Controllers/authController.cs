using System;
using Data;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Db _Db;
        private readonly IConfiguration _configuration;

        public AuthController(Db Db, IConfiguration configuration)
        {
            _Db = Db;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReturn>> Register(UserRegistration RegData)
        {
            if (await _Db.Users.AnyAsync(user => user.Email == RegData.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(RegData.Password);

            User user = new User(RegData.Name, RegData.Surname, RegData.Email, passwordHash);

            _Db.Users.Add(user);
            await _Db.SaveChangesAsync();

            string jwToken = CreateToken(user);

            return Ok(new UserReturn(user.Id, user.Name, user.Surname, user.Email, jwToken));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserReturn>> Login(UserLogin LoginData)
        {
            User? user = await _Db.Users.FirstOrDefaultAsync(u => u.Email == LoginData.Email);
            if (user == null)
            {
                return NotFound("User with this email does not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(LoginData.Password, user.PasswordHash))
            {
                return BadRequest("Password is not correct");
            }

            string jwToken = CreateToken(user);

            return Ok(new UserReturn(user.Id, user.Name, user.Surname, user.Email, jwToken));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserReturn>> Me()
        {
            string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User Id is not correct");
            }

            User user = _Db.Users.Find(userId);
            return new UserReturn(user.Id, user.Name, user.Surname, user.Email);
        }

        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}