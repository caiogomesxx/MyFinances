using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFinance.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = config;
        }

        [HttpPost("CadastrarUsuario")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] TbUsuario user)
        {

            _unitOfWork.Users.CadastrarUser(user);


            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.IdUsuario.ToString()),
                        new Claim("DisplayName",user.DsNome),
                        new Claim("Email", user.DsEmail),
                        new Claim("Password", user.DsSenha),
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
        [Authorize]
        [HttpGet]
        public List<TbUsuario> Get()
        {
            List<TbUsuario> response = _unitOfWork.Users.GetAll().Result.ToList();
            return response;
        }


    }
}
