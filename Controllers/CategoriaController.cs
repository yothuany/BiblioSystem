using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;
using BiblioSystem.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("categoria")]
    [Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _service;

        public CategoriaController(CategoriaService service)
        {
            _service = service;
        }
        /// <summary>
        /// Listagem de categorias, busca por ID ou nome, paginação e ordenação.
        /// </summary>

        // GET: /categoria/Consultar
        [HttpGet("Consultar")]
        public async Task<IActionResult> FindAll([FromQuery] CategoriaFilter filter)
        {
            try
            {
                var categoria = await _service.FindAll(filter);

                if (categoria.Data == null || !categoria.Data.Any())
                {
                    return Ok(new { mensagem = "Nenhuma categoria encontrada.", dados = categoria });
                }
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

        // POST: /categoria/Cadastrar
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] CategoriaDto novaCategoria)
        {
            try
            {
                var categoria = await _service.Create(novaCategoria);
                return Created("", new { mensagem = "Categoria cadastrada com sucesso!", dados = categoria });
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

        // PUT: /categoria/Editar/{id}
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoriaDto categoriaDto)
        {
            try
            {
                var categoria = await _service.Update(id, categoriaDto);
                return Ok(new { mensagem = "Categoria atualizada com sucesso!", dados = categoria });
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

        // DELETE: /categoria/Remover/{id}
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok(new { mensagem = "Categoria removida com sucesso!" });
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
    }
}