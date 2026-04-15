using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Controllers
{
    [Route("/livros")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly LivroService _service;

        public LivroController(LivroService service)
        {
            _service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var livros = await _service.FindAll();

                return Ok(livros);
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
                var livro = await _service.FindById(id);

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

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] LivroDto novoLivro)
        {
            try
            {
                var livro = await _service.Create(novoLivro);

                return Created("", livro);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LivroUpdateDto livroDto)
        {
            try
            {
                var livro = await _service.Update(id, livroDto);

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