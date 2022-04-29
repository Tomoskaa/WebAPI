using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApiProducts.Data;
using WebApiProducts.Models;

namespace WebApiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _config;
        public LoginController(UserDbContext userDbContext, IConfiguration config)
        {
            _context = userDbContext;
            _config = config;
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.userModels.AsQueryable();
            return Ok(users);
        }
        [HttpPost("signUp")]
        public IActionResult SignUp([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                _context.userModels.Add(user);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "User Added Successfully"
                });
            }
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                var getUser = _context.userModels
                    .Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();

                if (getUser != null)
                {
                    var token = GenerateToken(getUser.Username);
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Logged In Successfully",
                        UserData = user.FullName,
                        JwtToken = token
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User Not Found"
                    });
                }
            }
        }

        [HttpDelete("delete_user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.userModels.Find(id);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User Not Found"
                });
            }
            else
            {
                _context.Remove(user);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "User Deleted Successfully"
                });
            }
        }



        private string GenerateToken(string username)
        {
            var handlertoken = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, username),
                new Claim("ProductWebAPI", "Products")
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credential
                );
            return handlertoken.WriteToken(token);
        }
    }
}
