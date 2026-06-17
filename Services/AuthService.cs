using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiblioSystem.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;

        private readonly AppDbContext _context;

        public AuthService(
            IConfiguration config,
            AppDbContext context
        )
        {
            _config = config;

            _context = context;
        }


        public async Task<
            string
        > Login(
            LoginDto data
        )
        {
            var usuario =
                await _context
                .Usuarios
                .FirstOrDefaultAsync(
                    x =>
                    x.Email
                    ==
                    data.Email
                );


            if (
                usuario
                is null
                ||
                usuario.Senha
                !=
                data.Password
            )
            {
                throw new ErrorServiceException(
                    "Login inválido",

                    c =>
                    c.Unauthorized()
                );
            }


            return GenerateToken(
                usuario
            );
        }


        private string
        GenerateToken(
            Usuario usuario
        )
        {
            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _config["Jwt:Key"]!
                    )
                );

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );


            var token =
                new JwtSecurityToken(
                    issuer:
                    _config["Jwt:Issuer"],

                    audience:
                    _config["Jwt:Audience"],

                    claims:
                    new[]
                    {
                        new Claim(
                            ClaimTypes.Email,
                            usuario.Email
                        )
                    },

                    expires:
                    DateTime.Now
                    .AddHours(8),

                    signingCredentials:
                    creds
                );


            return
                new JwtSecurityTokenHandler()
                .WriteToken(
                    token
                );
        }
    }
}