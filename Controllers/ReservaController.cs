using BiblioSystem.Dtos.Reserva;
using BiblioSystem.Exceptions;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservaController(ReservaService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try { return Ok(await service.GetByIdAsync(id)); }
        catch (Exceptions.Exceptions ex) { return base.NotFound(new { ex.Message }); }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservaCreateDto dto)
    {
        try
        {
            var reserva = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reserva.IdReserva }, reserva);
        }
        catch (Exceptions.Exceptions ex) { return base.NotFound(new { ex.Message }); }
        catch (BusinessException ex) { return base.Conflict(new { ex.Message }); }
    }

    [HttpPatch("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        try { await service.CancelarAsync(id); return base.NoContent(); }
        catch (Exceptions.Exceptions ex) { return base.NotFound(new { ex.Message }); }
        catch (BusinessException ex) { return base.Conflict(new { ex.Message }); }
    }
}
