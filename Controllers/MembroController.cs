using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("membros")]
    [Authorize]
    public class MembroController : ControllerBase
    {
        private readonly MembroService _service;

        public MembroController(
            MembroService service
        )
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
        Create(
            MembroDto data
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