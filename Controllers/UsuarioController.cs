using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace BiblioSystem.Controllers
{
    [Route("usuarios")]
    [ApiController]
   // [Authorize(Roles = "Administrador")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public UsuarioController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Consultar")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var usuarios = await _context.Usuario.ToListAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] UsuarioDto dto)
        {
            try
            {
                var emailExiste = await _context.Usuario.AnyAsync(x => x.Email == dto.Email);
                if (emailExiste)
                {
                    return BadRequest("Este e-mail já está sendo utilizado no sistema.");
                }

                var usuario = _mapper.Map<Usuario>(dto);

                usuario.Senha = _passwordHasher.HashPassword(usuario, dto.Senha);

                await _context.Usuario.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return Created("", new { mensagem = "Usuário cadastrado com sucesso!", dados = usuario});
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("Editar{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDto dto)
        {
            try
            {
                var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);
                if (usuario is null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                var emailExiste = await _context.Usuario.AnyAsync(x => x.Email == dto.Email && x.Id != id);
                if (emailExiste)
                {
                    return BadRequest("Este e-mail já pertence a outro funcionário.");
                }

                _mapper.Map(dto, usuario);

                if (!string.IsNullOrWhiteSpace(dto.Senha))
                {
                    usuario.Senha = _passwordHasher.HashPassword(usuario, dto.Senha);
                }

                _context.Usuario.Update(usuario);
                await _context.SaveChangesAsync();

                return Ok(new { mensagem = "Usuário atualizado com sucesso!", dados = usuario });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("Remover{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var idUsuarioLogadoStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(idUsuarioLogadoStr) || !int.TryParse(idUsuarioLogadoStr, out int idUsuarioLogado))
                {
                    idUsuarioLogadoStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                    int.TryParse(idUsuarioLogadoStr, out idUsuarioLogado);
                }

                if (id == idUsuarioLogado)
                {
                    return BadRequest("Operação inválida. Você não pode deletar o seu próprio perfil.");
                }

                var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Id == id);
                if (usuario is null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                _context.Usuario.Remove(usuario);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuário removido com sucesso do sistema de biblioteca!" });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}