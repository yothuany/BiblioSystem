using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(
            UsuarioService service
        )
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult>
        Create(
            UsuarioDto data
        )
        {
            var usuario =
                await _service
                .Create(
                    data
                );

            return Created(
                "",
                usuario
            );
        }
    }
}