using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;
using BiblioSystem.Exceptions;
using Microsoft.AspNetCore.Authorization;


namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("livro")]
    [Authorize]
    public class LivroController : ControllerBase
    {
        private readonly LivroService _service;

        public LivroController(LivroService service)
        {
            _service = service;
        }

        /// <summary>
        /// Listagem de livros, busca por ID, título, ISBN ou autor, paginação e ordenação.
        /// </summary>
       
        // GET: /livro/Consultar
        [HttpGet("Consultar")]
        public async Task<IActionResult> FindAll([FromQuery] LivroFilter filter)
        {
            try
            {
                var livro = await _service.FindAll(filter);

                if (livro.Data == null || !livro.Data.Any())
                {
                    return Ok(new { mensagem = "Nenhum livro encontrado.", dados = livro });
                }
                return Ok(livro);
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

        // POST: /livro/Cadastrar
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] LivroDto novoLivro)
        {
            try
            {
                var livro = await _service.Create(novoLivro);
                return Created("", new { mensagem = "Livro cadastrado com sucesso!", dados = livro });
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

        // POST: /livro/Editar/{id}
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LivroDto livroDto)
        {
            try
            {
                var livro = await _service.Update(id, livroDto);
                return Ok(new { mensagem = "Livro atualizado com sucesso!", dados = livro });
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

        // DELETE: /livro/Remover/{id}
        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok(new { mensagem = "Livro removido com sucesso!" });
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