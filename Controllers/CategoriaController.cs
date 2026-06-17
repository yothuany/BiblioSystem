using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [Route("/categorias")]
    [ApiController]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _service;

        public CategoriaController(CategoriaService service)
        {
            _service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var categorias = await _service.FindAll();
                return Ok(categorias);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var categoria = await _service.FindById(id);
                return Ok(categoria);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CategoriaDto novaCategoria)
        {
            try
            {
                var categoria = await _service.Create(novaCategoria);
                return Created("", categoria);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoriaDto categoriaDto)
        {
            try
            {
                var categoria = await _service.Update(id, categoriaDto);
                return Ok(categoria);
            }
            catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
