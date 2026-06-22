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
    public class LivroService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LivroService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<LivroResponseDto>> FindAll(LivroFilter filter)
        {
            try
            {
                var query = _context.Livro
                    .Include(x => x.Autores)
                    .Include(x => x.Categorias) 
                    .AsQueryable();

                if (filter.Search is not null)
                {
                    var searchLimpo = filter.Search.Replace("-", "").Replace(" ", "");
                    bool isbnCompleto = searchLimpo.Length == 10 || searchLimpo.Length == 13;

                    if (int.TryParse(filter.Search, out int idBuscado))
                    {
                        query = query.Where(x => x.Id == idBuscado
                                              || x.Titulo.Contains(filter.Search)
                                              || (isbnCompleto && x.Isbn == filter.Search)
                                              || x.Autores.Any(a => a.Nome.Contains(filter.Search)));
                    }
                    else
                    {
                        query = query.Where(x => x.Titulo.Contains(filter.Search)
                                              || x.Autores.Any(a => a.Nome.Contains(filter.Search))
                                              || (isbnCompleto && x.Isbn == filter.Search));
                    }
                }

                if (!string.IsNullOrEmpty(filter.Autor))
                {
                    query = query.Where(x => x.Autores.Any(a => a.Nome.Contains(filter.Autor)));
                }

                return await Paginate<Livro>.Set<LivroResponseDto>(query, filter, _mapper);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LivroResponseDto> Create(LivroDto data)
        {
            try
            {
                var autores = await _context.Autor
                    .Where(a => data.AutoresIds.Contains(a.Id))
                    .ToListAsync();

                if (autores.Count != data.AutoresIds.Count)
                {
                    throw new ErrorServiceException("Um ou mais Autores informados não foram encontrados",
                        c => c.BadRequest(new { message = "Um ou mais Autores informados não foram encontrados." }));
                }

                var categorias = await _context.Categoria
                    .Where(c => data.CategoriasIds.Contains(c.Id))
                    .ToListAsync();

                if (categorias.Count != data.CategoriasIds.Count)
                {
                    throw new ErrorServiceException("Uma ou mais Categorias informadas não foram encontradas",
                        c => c.BadRequest(new { message = "Uma ou mais Categorias informadas não foram encontradas." }));
                }

                var livro = _mapper.Map<Livro>(data);
                livro.Autores = autores;
                livro.Categorias = categorias;

                await _context.Livro.AddAsync(livro);
                await _context.SaveChangesAsync();

                return _mapper.Map<LivroResponseDto>(livro);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Livro> FindById(int id)
        {
            try
            {
                var livro = await _context.Livro
                    .Include(x => x.Autores)
                    .Include(x => x.Categorias)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (livro is null)
                {
                    throw new ErrorServiceException($"Livro {id} não encontrado",
                        c => c.NotFound(new { message = $"Livro #{id} não encontrado" }));
                }

                return livro;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LivroResponseDto> Update(int id, LivroDto data)
        {
            try
            {
                var livro = await FindById(id);

                var autores = await _context.Autor
                    .Where(a => data.AutoresIds.Contains(a.Id))
                    .ToListAsync();

                if (autores.Count != data.AutoresIds.Count)
                {
                    throw new ErrorServiceException("Um ou mais Autores informados não foram encontrados",
                        c => c.BadRequest(new { message = "Um ou mais Autores informados não foram encontrados." }));
                }

                var categorias = await _context.Categoria
                    .Where(c => data.CategoriasIds.Contains(c.Id))
                    .ToListAsync();

                if (categorias.Count != data.CategoriasIds.Count)
                {
                    throw new ErrorServiceException("Uma ou mais Categorias informadas não foram encontradas",
                        c => c.BadRequest(new { message = "Uma ou mais Categorias informadas não foram encontradas." }));
                }

                _mapper.Map(data, livro);

                livro.Autores!.Clear();
                foreach (var autor in autores) livro.Autores.Add(autor);

                livro.Categorias!.Clear();
                foreach (var categoria in categorias) livro.Categorias.Add(categoria);

                _context.Livro.Update(livro);
                await _context.SaveChangesAsync();

                return _mapper.Map<LivroResponseDto>(livro);
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
                var livro = await FindById(id);

                _context.Livro.Remove(livro);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}