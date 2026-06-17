using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("reservas")]
    [Authorize]
    public class ReservaController : ControllerBase
    {
        private readonly ReservaService _service;

        public ReservaController(
            ReservaService service
        )
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
        Create(
            ReservaDto data
        )
        {
            return Created(
                "",
                await _service
                .Create(
                    data
                )
            );
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult>
        Cancelar(
            int id
        )
        {
            return Ok(
                await _service
                .Cancelar(
                    id
                )
            );
        }
    }
}