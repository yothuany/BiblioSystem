using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("exemplares")]
    [Authorize]
    public class ExemplarController : ControllerBase
    {
        private readonly ExemplarService _service;

        public ExemplarController(
            ExemplarService service
        )
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult>
        FindAll(
            [FromQuery]
            ExemplarFilter filter
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
            ExemplarDto data
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

        [HttpPatch("{id}/status")]
        public async Task<IActionResult>
        UpdateStatus(
            int id,
            string status
        )
        {
            return Ok(
                await _service
                .UpdateStatus(
                    id,
                    status
                )
            );
        }
    }
}