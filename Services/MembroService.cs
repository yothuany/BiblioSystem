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
    public class MembroService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MembroService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<MembroResponseDto>> FindAll(MembroFilter filter)
        {
            try
            {
                var query = _context.Membro.AsQueryable();

                if (filter.Search is not null)
                {
                    if (int.TryParse(filter.Search, out int idBuscado))
                    {
                        query = query.Where(x => x.Id == idBuscado);
                    }
                    else
                    {
                        query = query.Where(x => x.Nome.Contains(filter.Search) ||
                                                 x.Email.Contains(filter.Search) ||
                                                 x.Cpf.Contains(filter.Search));
                    }
                }
                return await Paginate<Membro>.Set<MembroResponseDto>(query, filter, _mapper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Membro> Create(MembroDto data)
        {
            try
            {
                var dadosDuplicados = await _context.Membro
                    .AnyAsync(m => m.Email == data.Email || m.Cpf == data.Cpf);

                if (dadosDuplicados)
                {
                    throw new ErrorServiceException("E-mail ou CPF já cadastrados.",
                        c => c.BadRequest(new { message = "Já existe um membro cadastrado com este e-mail ou CPF." }));
                }

                var membro = _mapper.Map<Membro>(data);

                await _context.Membro.AddAsync(membro);
                await _context.SaveChangesAsync();

                return membro;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Membro> FindById(int id)
        {
            try
            {
                var membro = await _context.Membro.FirstOrDefaultAsync(x => x.Id == id);

                if (membro is null)
                {
                    throw new ErrorServiceException($"Membro {id} não encontrado",
                        c => c.NotFound(new { message = $"Membro #{id} não encontrado" }));
                }

                return membro;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Membro> Update(int id, MembroDto data)
        {
            try
            {
                var membro = await FindById(id);

                var dadosDuplicados = await _context.Membro
                    .AnyAsync(m => (m.Email == data.Email || m.Cpf == data.Cpf) && m.Id != id);

                if (dadosDuplicados)
                {
                    throw new ErrorServiceException("E-mail ou CPF já cadastrados.",
                        c => c.BadRequest(new { message = "Não é possível atualizar: O e-mail ou CPF informado pertence a outro membro." }));
                }

                _mapper.Map(data, membro);

                _context.Membro.Update(membro);
                await _context.SaveChangesAsync();

                return membro;
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
                var membro = await FindById(id);

                var possuiReservasAtivas = await _context.Reserva
                    .AnyAsync(r => r.MembroId == id && r.Status == "Ativa");

                if (possuiReservasAtivas)
                {
                    throw new ErrorServiceException("Não é possível remover o membro.",
                        c => c.BadRequest(new { message = $"O membro '{membro.Nome}' não pode ser removido porque possui reservas ativas em andamento." }));
                }

                _context.Membro.Remove(membro);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}