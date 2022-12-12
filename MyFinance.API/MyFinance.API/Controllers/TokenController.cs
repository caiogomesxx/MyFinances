using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFinance.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public TokenController(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _configuration = config;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        

        [HttpPost]
        public async Task<IActionResult> Post(TbUsuario _userData)
        {
            if (_userData != null && _userData.DsEmail != null && _userData.DsSenha != null)
            {
                var user = await GetUser(_userData.DsEmail, _userData.DsSenha);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.IdUsuario.ToString()),
                        new Claim("DisplayName", user.DsNome),
                        new Claim("Email", user.DsEmail),
                        new Claim("Password", user.DsSenha),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(100),
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

        private async Task<TbUsuario> GetUser(string email, string password)
        {
            return await _unitOfWork.Users.GetUserByEmail(email, password);
        }
    }
}
