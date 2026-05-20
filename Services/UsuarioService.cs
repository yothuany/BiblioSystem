using BiblioSystem.DataContexts;
using BiblioSystem.Dtos.Usuario;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services;

public class UsuarioService(AppDbContext db)
{
    // RF11 - Autenticação por e-mail e senha
    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await db.Usuarios
            .Include(u => u.Membro)
            .FirstOrDefaultAsync(u => u.Email == dto.Email)
            ?? throw new UnauthorizedException("E-mail ou senha inválidos.");

        var senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha);
        if (!senhaValida)
            throw new UnauthorizedException("E-mail ou senha inválidos.");

        // Retorna token simples (pode ser substituído por JWT)
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        return new LoginResponseDto(token, usuario.Membro.Nome, usuario.Email);
    }

    public async Task<Usuario> CreateAsync(UsuarioCreateDto dto)
    {
        var emailExistente = await db.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (emailExistente)
            throw new BusinessException("E-mail já cadastrado.");

        var membroExiste = await db.Membros.AnyAsync(m => m.IdMembro == dto.MembroIdMembro);
        if (!membroExiste)
            throw new NotFoundException($"Membro com id {dto.MembroIdMembro} não encontrado.");

        var usuario = new Usuario
        {
            Email = dto.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            MembroIdMembro = dto.MembroIdMembro
        };

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();
        return usuario;
    }
}
