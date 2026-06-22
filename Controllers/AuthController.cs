using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiblioSystem.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher = new();

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        ///<summary>
        /// Realiza o login do usuário e retorna o Token JWT.
        /// </summary>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Email.Equals(user.Email));

            if (usuario is null)
            {
                return BadRequest("E-mail e/ou senha incorretos");
            }

            var verificacao = _hasher.VerifyHashedPassword(usuario, usuario.Senha, user.Senha);

            if (verificacao == PasswordVerificationResult.Failed)
            {
                return BadRequest("E-mail e/ou senha incorretos");
            }

            var token = GerarJwtToken(usuario);

            return Ok(new { token });
        }

        private string GerarJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim("Name", usuario.Nome),
                new Claim("Email", usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}