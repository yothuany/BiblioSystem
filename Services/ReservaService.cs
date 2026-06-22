using AutoMapper;
using BiblioSystem.Controllers.Filters;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Dtos.Responses;
using BiblioSystem.Exceptions;
using BiblioSystem.Helpers.Paginated;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services
{
    public class ReservaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReservaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<ReservaResponseDto>> FindAll(ReservaFilter filter)
        {
            try
            {
                var query = _context.Reserva
                    .Include(x => x.Livro)
                    .Include(x => x.Membro)
                    .AsQueryable();

                if (filter.Search is not null)
                {
                    if (int.TryParse(filter.Search, out int idBuscado))
                    {
                        query = query.Where(x => x.Id == idBuscado);
                    }
                    else
                    {
                        query = query.Where(x => x.Status.Contains(filter.Search) ||
                                                 x.Livro.Titulo.Contains(filter.Search) ||
                                                 x.Membro.Nome.Contains(filter.Search));
                    }
                }
                return await Paginate<Reserva>.Set<ReservaResponseDto>(query, filter, _mapper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Reserva> Create(ReservaDto data)
        {
            try
            {
                var livro = await _context.Livro.FirstOrDefaultAsync(l => l.Isbn == data.IsbnLivro);
                if (livro is null)
                {
                    throw new ErrorServiceException($"Livro com ISBN {data.IsbnLivro} não encontrado",
                        c => c.BadRequest(new { message = $"Não é possível reservar: Livro com ISBN {data.IsbnLivro} não existe." }));
                }

                var membro = await _context.Membro.FirstOrDefaultAsync(m => m.Cpf == data.CpfMembro);
                if (membro is null)
                {
                    throw new ErrorServiceException($"Membro com CPF {data.CpfMembro} não encontrado",
                        c => c.BadRequest(new { message = $"Não é possível reservar: Membro com CPF {data.CpfMembro} não existe." }));
                }

                var reserva = new Reserva
                {
                    LivroId = livro.Id,       
                    MembroId = membro.Id,     
                    DataReserva = DateTime.Now.Date,
                    Status = "Ativa"
                };

                await _context.Reserva.AddAsync(reserva);
                await _context.SaveChangesAsync();

                return reserva;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Reserva> FindById(int id)
        {
            try
            {
                var reserva = await _context.Reserva
                    .Include(x => x.Livro)
                    .Include(x => x.Membro)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (reserva is null)
                {
                    throw new ErrorServiceException($"Reserva {id} não encontrada",
                        c => c.NotFound(new { message = $"Reserva #{id} não encontrada" }));
                }

                return reserva;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Reserva> UpdateStatus(int id, string novoStatus)
        {
            try
            {
                var reserva = await FindById(id);

                reserva.Status = novoStatus;

                _context.Reserva.Update(reserva);
                await _context.SaveChangesAsync();

                return reserva;
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
                var reserva = await FindById(id);

                _context.Reserva.Remove(reserva);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}