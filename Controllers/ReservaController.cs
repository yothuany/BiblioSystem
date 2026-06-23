using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Mvc;
using BiblioSystem.Exceptions;
using Microsoft.AspNetCore.Authorization;


namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("reserva")]
    [Authorize]
    public class ReservaController : ControllerBase
    {
        private readonly ReservaService _service;

        public ReservaController(ReservaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Listagem de reservas, busca por ID, status, livro ou membro, paginação e ordenação.
        /// </summary>

        // GET: /reserva/Consultar
        [HttpGet("Consultar")]
        public async Task<IActionResult> FindAll([FromQuery] ReservaFilter filter)
        {
            try
            {
                var reserva = await _service.FindAll(filter);

                if (reserva.Data == null || !reserva.Data.Any())
                {
                    return Ok(new { mensagem = "Nenhuma reserva encontrada.", dados = reserva });
                }
                return Ok(reserva);
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

        // POST: /reserva/Cadastrar
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] ReservaDto novaReserva)
        {
            try
            {
                var reserva = await _service.Create(novaReserva);
                return Created("", new { mensagem = "Reserva realizada com sucesso!", dados = reserva });
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

        // PUT: /reserva/Cancelar
        [HttpPut("Cancelar/{id}")]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var reserva = await _service.UpdateStatus(id, "Cancelada");
                return Ok(new { mensagem = "Reserva cancelada com sucesso!", dados = reserva });
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

        // DELETE: /reserva/Remover/{id}
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok(new { mensagem = "Histórico de reserva removido com sucesso!" });
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