using BiblioSystem.Dtos.Usuario;
using BiblioSystem.Exceptions;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController(UsuarioService service) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try { return Ok(await service.LoginAsync(dto)); }
        catch (UnauthorizedException ex) { return Unauthorized(new { ex.Message }); }
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] UsuarioCreateDto dto)
    {
        try
        {
            var usuario = await service.CreateAsync(dto);
            return base.Created(string.Empty, new { usuario.IdUsuario, usuario.Email });
        }
        catch (BusinessException ex) { return base.Conflict(new { ex.Message }); }
        catch (Exceptions.Exceptions ex) { return base.NotFound(new { ex.Message }); }
    }
}
