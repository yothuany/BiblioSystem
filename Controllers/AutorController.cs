using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Models;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BiblioSystem.Exceptions;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("autor")]
    [Authorize]
    public class AutorController : ControllerBase
    {
        private readonly AutorService _service;

        public AutorController(AutorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Listagem de autores, busca por ID ou nome, paginação e ordenação.
        /// </summary>
      
        // GET: /autor/Consultar
        [HttpGet("Consultar")]
        public async Task<IActionResult> FindAll([FromQuery] AutorFilter filter)
        {
            try
            {
                var autor = await _service.FindAll(filter);

                if (autor.Data == null || !autor.Data.Any())
                {
                    return Ok(new { mensagem = "Nenhum autor encontrado.", dados = autor });
                }
                return Ok(autor);
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

        // POST: /autor/Cadastrar
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] AutorDto novoAutor)
        {
            try
            {
                var autor = await _service.Create(novoAutor);
                return Created("", new { mensagem = "Autor cadastrado com sucesso!", dados = autor });
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

        // POST: /autor/Editar
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AutorDto autorDto)
        {
            try
            {
                var autor = await _service.Update(id, autorDto);
                return Ok(new { mensagem = "Autor atualizado com sucesso!", dados = autor });
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

        // DELETE: /autor/Remover
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> Remove( int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok(new { mensagem = "Autor removido com sucesso!" });
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