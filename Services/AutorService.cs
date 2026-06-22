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
    public class AutorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AutorService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<AutorResponseDto>> FindAll(AutorFilter filter)
        {
            try
            {
                var query = _context.Autor.AsQueryable();

                if (filter.Search is not null)
                {
                    if (int.TryParse(filter.Search, out int idBuscado))
                    {
                        query = query.Where(x => x.Id == idBuscado);
                    }
                    else
                    {
                        query = query.Where(x => x.Nome.Contains(filter.Search));
                    }
                }
                return await Paginate<Autor>.Set<AutorResponseDto>(query, filter, _mapper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Autor> Create(AutorDto data)
        {
            try
            {
                var autor = _mapper.Map<Autor>(data);

                await _context.Autor.AddAsync(autor);
                await _context.SaveChangesAsync();

                return autor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Autor> FindById(int id)
        {
            try
            {
                var autor = await _context.Autor.FirstOrDefaultAsync(x => x.Id == id);

                if (autor is null)
                {
                    throw new ErrorServiceException($"Autor {id} não encontrado",
                        c => c.NotFound(new { message = $"Autor #{id} não encontrado" }));
                }

                return autor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Autor> Update(int id, AutorDto data)
        {
            try
            {
                var autor = await FindById(id);

                _mapper.Map(data, autor);

                _context.Autor.Update(autor);
                await _context.SaveChangesAsync();

                return autor;
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
                var autor = await FindById(id);

                var possuiLivros = await _context.Livro
                    .AnyAsync(l => l.Autores.Any(a => a.Id == id));

                if (possuiLivros)
                {
                    throw new ErrorServiceException($"Não é possível remover o autor porque ele possui livros vinculados.",
                        c => c.BadRequest(new { message = $"O autor '{autor.Nome}' não pode ser removido porque está vinculado a um ou mais livros." }));
                }

                _context.Autor.Remove(autor);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}