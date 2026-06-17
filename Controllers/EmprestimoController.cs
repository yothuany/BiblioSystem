using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("emprestimos")]
    [Authorize]
    public class EmprestimoController : ControllerBase
    {
        private readonly EmprestimoService _service;

        public EmprestimoController(
            EmprestimoService service
        )
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
        Create(
            EmprestimoDto data
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

        [HttpPatch("{id}/devolucao")]
        public async Task<IActionResult>
        RegistrarDevolucao(
            int id
        )
        {
            return Ok(
                await _service
                .RegistrarDevolucao(
                    id
                )
            );
        }
    }
}