using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Domain.DTO;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFinance.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    
   

    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        public LoginController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = config;
 
        }

        [HttpPost("FazerLogin")]
        public async Task<IActionResult> FazerLogin([FromBody] LoginDTO login)
        {
            if (login != null && login.Email != null && login.Email != null)
            {
                var user = _unitOfWork.Users.GetUserByEmail(login.Email, login.Password);
                
                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Result.IdUsuario.ToString()),
                        new Claim("DisplayName", user.Result.DsNome),
                        new Claim("Email", user.Result.DsEmail),
                        new Claim("Password", user.Result.DsSenha),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(7),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
