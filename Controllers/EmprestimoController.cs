using BiblioSystem.Controllers.Filters;
using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using BiblioSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioSystem.Controllers
{
    [ApiController]
    [Route("emprestimo")]
    [Authorize]
    public class EmprestimoController : ControllerBase
    {
        private readonly EmprestimoService _service;

        public EmprestimoController(EmprestimoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Listagem de empréstimos. Busca por ID, status, código do exemplar, nome ou CPF do membro, paginação e ordenação.
        /// </summary>

        // GET: /emprestimo/Consultar
        [HttpGet("Consultar")]
        public async Task<IActionResult> FindAll([FromQuery] EmprestimoFilter filter)
        {
            try
            {
                var emprestimos = await _service.FindAll(filter);

                if (emprestimos.Data == null || !emprestimos.Data.Any())
                {
                    return Ok(new { mensagem = "Nenhum empréstimo encontrado.", dados = emprestimos });
                }
                return Ok(emprestimos);
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

        // POST: /emprestimo/Cadastrar
        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Create([FromBody] EmprestimoDto novoEmprestimo)
        {
            try
            {
                var emprestimo = await _service.Create(novoEmprestimo);
                return Created("", new { mensagem = "Empréstimo cadastrado com sucesso!", dados = emprestimo });
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

        // PUT: /emprestimo/Devolver/{id}
        [HttpPut("Devolver/{id}")]
        public async Task<IActionResult> Devolver(int id)
        {
            try
            {
                var emprestimo = await _service.RegistrarDevolucao(id);

                var resultadoClean = new
                {
                    idEmprestimo = emprestimo.Id,
                    status = emprestimo.Status,
                    dataDevolucao = emprestimo.DataDevolucao,
                    valorMulta = emprestimo.ValorMulta
                };

                return Ok(new
                {
                    mensagem = "Devolução registrada com sucesso!",
                    dados = resultadoClean
                });
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

        // DELETE: /emprestimo/Remover/{id}
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);
                return Ok(new { mensagem = "Histórico de empréstimo removido com sucesso!" });
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