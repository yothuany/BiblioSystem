using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;
using BiblioSystem.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("membro")]
    [Authorize]
    public class MembroController : ControllerBase
    {
        private readonly MembroService _service;

        public MembroController(MembroService service)
        {
            _service = service;
        }

        /// <summary>
        /// Listagem de membros, busca por ID, nome, e-mail ou CPF, paginação e ordenação.
        /// </summary>

        // GET: /membro/Consultar
        [HttpGet("Consultar")]
        public async Task<IActionResult> FindAll([FromQuery] MembroFilter filter)
        {
            try
            {
                var membro = await _service.FindAll(filter);

                if (membro.Data == null || !membro.Data.Any())
                {
                    return Ok(new { mensagem = "Nenhum membro encontrado.", dados = membro });
                }
                return Ok(membro);
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

        // POST: /membro/Cadastrar
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] MembroDto novoMembro)
        {
            try
            {
                var membro = await _service.Create(novoMembro);
                return Created("", new { mensagem = "Membro cadastrado com sucesso!", dados = membro });
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

        // PUT: /membro/Editar/{id}
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MembroDto membroDto)
        {
            try
            {
                var membro = await _service.Update(id, membroDto);
                return Ok(new { mensagem = "Membro atualizado com sucesso!", dados = membro });
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

        // DELETE: /membro/Remover/{id}
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok(new { mensagem = "Membro removido com sucesso!" });
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