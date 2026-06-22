using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;
using BiblioSystem.Exceptions;
using Microsoft.AspNetCore.Authorization;


namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("exemplar")]
    [Authorize]
    public class ExemplarController : ControllerBase
    {
        private readonly ExemplarService _service;

        public ExemplarController(ExemplarService service)
        {
            _service = service;
        }

        /// <summary>
        /// Listagem de exemplares, busca por ID, código ou status, paginação e ordenação.
        /// </summary>
   
        // GET: /exemplar/Consultar
        [HttpGet("Consultar")]
        public async Task<IActionResult> FindAll([FromQuery] ExemplarFilter filter)
        {
            try
            {
                var exemplar = await _service.FindAll(filter);

                if (exemplar.Data == null || !exemplar.Data.Any())
                {
                    return Ok(new { mensagem = "Nenhum exemplar encontrado.", dados = exemplar });
                }
                return Ok(exemplar);
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

        // POST: /exemplar/Cadastrar
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] ExemplarDto novoExemplar)
        {
            try
            {
                var exemplar = await _service.Create(novoExemplar);
                return Created("", new { mensagem = "Exemplar cadastrado com sucesso!", dados = exemplar });
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

        // PUT: /exemplar/Editar/{id}
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExemplarDto exemplarDto)
        {
            try
            {
                var exemplar = await _service.Update(id, exemplarDto);
                return Ok(new { mensagem = "Exemplar atualizado com sucesso!", dados = exemplar });
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

        // DELETE: /exemplar/Remover/{id}
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok(new { mensagem = "Exemplar removido com sucesso!" });
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