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
    public class CategoriaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<CategoriaResponseDto>> FindAll(CategoriaFilter filter)
        {
            try
            {
                var query = _context.Categoria.AsQueryable();

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
                return await Paginate<Categoria>.Set<CategoriaResponseDto>(query, filter, _mapper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Categoria> Create(CategoriaDto data)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(data);

                await _context.Categoria.AddAsync(categoria);
                await _context.SaveChangesAsync();

                return categoria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Categoria> FindById(int id)
        {
            try
            {
                var categoria = await _context.Categoria.FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                {
                    throw new ErrorServiceException($"Categoria {id} não encontrada",
                        c => c.NotFound(new { message = $"Categoria #{id} não encontrada" }));
                }

                return categoria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Categoria> Update(int id, CategoriaDto data)
        {
            try
            {
                var categoria = await FindById(id);

                _mapper.Map(data, categoria);

                _context.Categoria.Update(categoria);
                await _context.SaveChangesAsync();

                return categoria;
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
                var categoria = await FindById(id);

                var possuiLivros = await _context.Livro
                    .AnyAsync(l => l.Categorias.Any(c => c.Id == id));

                if (possuiLivros)
                {
                    throw new ErrorServiceException($"Não é possível remover a categoria porque ela possui livros vinculados.",
                        c => c.BadRequest(new { message = $"A categoria '{categoria.Nome}' não pode ser removida porque está vinculada a um ou mais livros." }));
                }

                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}