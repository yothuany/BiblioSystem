using BiblioSystem.Dtos.Livro;
using BiblioSystem.Exceptions;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivroController(LivroService service) : ControllerBase
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

    // RF10 - Pesquisa avançada no catálogo
    [HttpGet("catalogo")]
    public async Task<IActionResult> Pesquisar(
        [FromQuery] string? titulo,
        [FromQuery] string? autor,
        [FromQuery] string? categoria,
        [FromQuery] string? editora)
    {
        var livros = await service.PesquisarAsync(titulo, autor, categoria, editora);
        return Ok(livros);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LivroCreateDto dto)
    {
        try
        {
            var livro = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = livro.IdLivro }, livro);
        }
        catch (BusinessException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LivroUpdateDto dto)
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
