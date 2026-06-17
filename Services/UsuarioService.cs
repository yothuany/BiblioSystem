using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public UsuarioService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;

            _mapper = mapper;
        }


        public async Task<Usuario>
        Create(
            UsuarioDto data
        )
        {
            var existe =
                await _context
                .Usuarios
                .AnyAsync(
                    x =>
                    x.Email
                    ==
                    data.Email
                );

            if (existe)
            {
                throw new ErrorServiceException(
                    "Usuário já existe",

                    c =>
                    c.Conflict()
                );
            }


            var usuario =
                _mapper
                .Map<Usuario>(
                    data
                );

            await _context
                .Usuarios
                .AddAsync(
                    usuario
                );

            await _context
                .SaveChangesAsync();

            return usuario;
        }
    }
}