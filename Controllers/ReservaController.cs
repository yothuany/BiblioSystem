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
        catch (NotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    // RF09 - Realizar reserva
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservaCreateDto dto)
    {
        try
        {
            var reserva = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reserva.IdReserva }, reserva);
        }
        catch (NotFoundException ex) { return NotFound(new { ex.Message }); }
        catch (BusinessException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpPatch("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        try { await service.CancelarAsync(id); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { ex.Message }); }
        catch (BusinessException ex) { return Conflict(new { ex.Message }); }
    }
}
