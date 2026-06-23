using AutoMapper;
using BiblioSystem.Controllers.Filters;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Dtos.Responses;
using BiblioSystem.Exceptions;
using BiblioSystem.Helpers.Paginated;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BiblioSystem.Services
{
    public class EmprestimoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EmprestimoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<EmprestimoResponseDto>> FindAll(EmprestimoFilter filter)
        {
            try
            {
                var query = _context.Emprestimo
                    .Include(x => x.Membro)
                    .Include(x => x.Exemplar)
                        .ThenInclude(e => e!.Livro)
                    .AsQueryable();

                if (filter.Search is not null)
                {
                    if (int.TryParse(filter.Search, out int numeroBuscado))
                    {
                        query = query.Where(x => x.Id == numeroBuscado ||
                                                 x.ExemplarId == numeroBuscado);
                    }
                    else
                    {
                        query = query.Where(x => x.Status.Contains(filter.Search) ||
                                                 x.Membro!.Nome.Contains(filter.Search) ||
                                                 x.Membro!.Cpf.Contains(filter.Search) ||
                                                 x.Exemplar!.Codigo.Contains(filter.Search));
                    }
                }

                return await Paginate<Emprestimo>.Set<EmprestimoResponseDto>(query, filter, _mapper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Emprestimo> Create(EmprestimoDto data)
        {
            try
            {
                var membro = await _context.Membro.FirstOrDefaultAsync(m => m.Cpf == data.CpfMembro);
                if (membro is null)
                {
                    throw new ErrorServiceException($"Membro com CPF {data.CpfMembro} não encontrado",
                        c => c.BadRequest(new { message = $"Não é possível emprestar: Membro com CPF {data.CpfMembro} não existe." }));
                }

                var exemplar = await _context.Exemplar.FirstOrDefaultAsync(e => e.Codigo == data.CodigoExemplar);
                if (exemplar is null)
                {
                    throw new ErrorServiceException($"Exemplar com Código {data.CodigoExemplar} não encontrado",
                        c => c.BadRequest(new { message = $"Não é possível emprestar: Exemplar com Código {data.CodigoExemplar} não existe." }));
                }

                if (exemplar.Status != "Disponível")
                {
                    throw new ErrorServiceException($"Exemplar indisponível",
                        c => c.BadRequest(new { message = $"Não é possível realizar o empréstimo: O exemplar '{data.CodigoExemplar}' encontra-se com o status '{exemplar.Status}'." }));
                }

                exemplar.Status = "Emprestado";
                _context.Exemplar.Update(exemplar);

                var emprestimo = new Emprestimo
                {
                    MembroId = membro.Id,
                    ExemplarId = exemplar.Id,
                    DataEmprestimo = DateTime.Now,
                    DataPrevistaDevolucao = DateTime.Now.AddDays(data.DiasDeEmprestimo).Date,
                    Status = "Ativo",
                    ValorMulta = 0.00m
                };

                await _context.Emprestimo.AddAsync(emprestimo);

                await _context.SaveChangesAsync();

                return emprestimo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Emprestimo> FindById(int id)
        {
            try
            {
                var emprestimo = await _context.Emprestimo
                    .Include(x => x.Membro)
                    .Include(x => x.Exemplar)
                        .ThenInclude(e => e!.Livro)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (emprestimo is null)
                {
                    throw new ErrorServiceException($"Empréstimo {id} não encontrado",
                        c => c.NotFound(new { message = $"Empréstimo #{id} não encontrado" }));
                }

                return emprestimo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Emprestimo> RegistrarDevolucao(int id)
        {
            try
            {
                var emprestimo = await _context.Emprestimo
                    .Include(e => e.Exemplar)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (emprestimo is null)
                {
                    throw new ErrorServiceException($"Empréstimo {id} não encontrado",
                        c => c.NotFound(new { message = $"Empréstimo #{id} não encontrado" }));
                }

                if (emprestimo.Status == "Devolvido")
                {
                    throw new ErrorServiceException($"Empréstimo {id} já foi devolvido",
                        c => c.BadRequest(new { message = "Este empréstimo já se encontra devolvido." }));
                }

                emprestimo.DataDevolucao = DateTime.Now;
                emprestimo.Status = "Devolvido";

                if (emprestimo.DataDevolucao.Value.Date > emprestimo.DataPrevistaDevolucao.Date)
                {
                    var diasAtraso = (emprestimo.DataDevolucao.Value.Date - emprestimo.DataPrevistaDevolucao.Date).Days;
                    emprestimo.ValorMulta = diasAtraso * 2.00m;
                }

                if (emprestimo.Exemplar is not null)
                {
                    emprestimo.Exemplar.Status = "Disponível";
                    _context.Exemplar.Update(emprestimo.Exemplar);
                }

                _context.Emprestimo.Update(emprestimo);
                await _context.SaveChangesAsync();

                return emprestimo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Remove(int id)
        {
            try
            {
                var emprestimo = await FindById(id);
                _context.Emprestimo.Remove(emprestimo);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}