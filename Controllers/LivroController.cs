using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("livros")]
    [Authorize]
    public class LivroController : ControllerBase
    {
        private readonly LivroService _service;

        public LivroController(
            LivroService service
        )
        {
            _service =
                service;
        }


        [HttpGet]
        public async Task<IActionResult>
        FindAll()
        {
            return Ok(
                await _service
                .FindAll()
            );
        }


        [HttpPost]
        public async Task<IActionResult>
        Create(
            LivroDto data
        )
        {
            var livro =
                await _service
                .Create(
                    data
                );

            return Created(
                "",
                livro
            );
        }
    }
}