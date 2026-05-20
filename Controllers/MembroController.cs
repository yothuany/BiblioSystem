using BiblioSystem.Dtos.Membro;
using BiblioSystem.Exceptions;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembroController(MembroService service) : ControllerBase
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MembroCreateDto dto)
    {
        try
        {
            var membro = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = membro.IdMembro }, membro);
        }
        catch (BusinessException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MembroUpdateDto dto)
    {
        try { return Ok(await service.UpdateAsync(id, dto)); }
        catch (NotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try { await service.DeleteAsync(id); return NoContent(); }
        catch (NotFoundException ex) { return NotFound(new { ex.Message }); }
    }
}
