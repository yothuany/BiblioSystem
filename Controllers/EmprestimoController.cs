using BiblioSystem.Dtos.Emprestimo;
using BiblioSystem.Exceptions;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmprestimoController(EmprestimoService service) : ControllerBase
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

    // RF06 - Realizar empréstimo
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmprestimoCreateDto dto)
    {
        try
        {
            var emprestimo = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = emprestimo.IdEmprestimo }, emprestimo);
        }
        catch (NotFoundException ex) { return NotFound(new { ex.Message }); }
        catch (BusinessException ex) { return Conflict(new { ex.Message }); }
    }

    // RF07 + RF08 - Registrar devolução com cálculo de multa
    [HttpPatch("{id}/devolver")]
    public async Task<IActionResult> Devolver(int id, [FromBody] EmprestimoDevolucaoDto dto)
    {
        try { return Ok(await service.RegistrarDevolucaoAsync(id, dto)); }
        catch (NotFoundException ex) { return NotFound(new { ex.Message }); }
        catch (BusinessException ex) { return Conflict(new { ex.Message }); }
    }
}
