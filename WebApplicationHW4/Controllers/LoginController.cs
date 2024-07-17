using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplicationHW4.Dtos;
using WebApplicationHW4.RSATools;
using WebApplicationHW4.Abstraction;

namespace WebApplicationHW4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;

        public LoginController(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("add_admin")]
        public ActionResult AddAdmin([FromBody] LoginDto login)
        {
            try
            {
                var user = new UserDto
                {
                    Name = login.Name,
                    Password = login.Password,
                    Role = UserRoleDto.Admin
                };
                _repository.AddUser(user);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("Add_user")]
        public ActionResult AddUser([FromBody] LoginDto login)
        {
            try
            {
                var user = new UserDto
                {
                    Name = login.Name,
                    Password = login.Password,
                    Role = UserRoleDto.User
                };
                _repository.AddUser(user);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                var role = _repository.CheckUser(login);

                var user = new UserDto
                {
                    Name = login.Name,
                    Password = login.Password,
                    Role = (UserRoleDto)role
                };

                var token = GenerateToken(user);

                return Ok(token);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }

        }

        private string GenerateToken(UserDto user)
        {
            var securityKey = new RsaSecurityKey(RSAExtensions.GetPrivateKey());

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
