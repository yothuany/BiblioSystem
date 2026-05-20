using BiblioSystem.Dtos.Exemplar;
using BiblioSystem.Exceptions;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExemplarController(ExemplarService service) : ControllerBase
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
    public async Task<IActionResult> Create([FromBody] ExemplarCreateDto dto)
    {
        try
        {
            var exemplar = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = exemplar.IdExemplar }, exemplar);
        }
        catch (Exceptions.Exceptions ex) { return base.NotFound(new { ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExemplarUpdateDto dto)
    {
        try { return Ok(await service.UpdateAsync(id, dto)); }
        catch (Exceptions.Exceptions ex) { return base.NotFound(new { ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try { await service.DeleteAsync(id); return base.NoContent(); }
        catch (Exceptions.Exceptions ex) { return base.NotFound(new { ex.Message }); }
    }
}
