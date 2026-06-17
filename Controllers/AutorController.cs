using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("autores")]
    [Authorize]
    public class AutorController : ControllerBase
    {
        private readonly AutorService _service;

        public AutorController(
            AutorService service
        )
        {
            _service = service;
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

        [HttpGet("v2")]
        public async Task<IActionResult>
        FindAllV2(
            [FromQuery]
            AutorFilter filter
        )
        {
            return Ok(
                await _service
                .FindAllV2(
                    filter
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult>
        Create(
            AutorDto data
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

        [HttpPut("{id}")]
        public async Task<IActionResult>
        Update(
            int id,
            AutorDto data
        )
        {
            return Ok(
                await _service
                .Update(
                    id,
                    data
                )
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>
        Remove(
            int id
        )
        {
            await _service
                .Remove(
                    id
                );

            return NoContent();
        }
    }
}