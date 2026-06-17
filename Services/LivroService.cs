using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public LivroService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<
            ICollection<LivroResponseDto>
        > FindAll()
        {
            try
            {
                return await _context
                    .Livros
                    .ProjectTo<LivroResponseDto>(
                        _mapper.ConfigurationProvider
                    )
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<
            PaginatedResponse<
                LivroResponseDto
            >
        > FindAllV2(
            LivroFilter filter
        )
        {
            try
            {
                var query =
                    _context
                    .Livros
                    .Include(
                        x => x.Categoria
                    )
                    .Include(
                        x => x.Autores
                    )
                    .AsQueryable();


                if (
                    filter.Search
                    is not null
                )
                {
                    query =
                        query.Where(
                            x =>
                            x.Titulo
                            .Contains(
                                filter.Search
                            )
                        );
                }


                if (
                    filter.CategoriaId
                    is not null
                )
                {
                    query =
                        query.Where(
                            x =>
                            x.CategoriaId
                            ==
                            filter.CategoriaId
                        );
                }


                if (
                    filter.Editora
                    is not null
                )
                {
                    query =
                        query.Where(
                            x =>
                            x.Editora
                            .Contains(
                                filter.Editora
                            )
                        );
                }


                if (
                    filter.Autor
                    is not null
                )
                {
                    query =
                        query.Where(
                            x =>
                            x.Autores!
                            .Any(
                                a =>
                                a.Nome
                                .Contains(
                                    filter.Autor
                                )
                            )
                        );
                }


                return await
                    Paginate<Livro>
                    .Set<
                        LivroResponseDto
                    >(
                        query,
                        filter,
                        _mapper
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<
            Livro
        > Create(
            LivroDto data
        )
        {
            try
            {
                var categoriaExiste =
                    await _context
                    .Categorias
                    .AnyAsync(
                        x =>
                        x.Id
                        ==
                        data.CategoriaId
                    );

                if (
                    !categoriaExiste
                )
                {
                    throw new ErrorServiceException(
                        "Categoria não encontrada",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                $"Categoria #{data.CategoriaId} não encontrada"
                            }
                        )
                    );
                }


                var livro =
                    _mapper
                    .Map<Livro>(
                        data
                    );

                await _context
                    .Livros
                    .AddAsync(
                        livro
                    );

                await _context
                    .SaveChangesAsync();

                return livro;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<
            Livro
        > FindById(
            int id
        )
        {
            try
            {
                var livro =
                    await _context
                    .Livros
                    .Include(
                        x => x.Categoria
                    )
                    .Include(
                        x => x.Autores
                    )
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        id
                    );

                if (
                    livro
                    is null
                )
                {
                    throw new ErrorServiceException(
                        $"Livro {id} não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                $"Livro #{id} não encontrado"
                            }
                        )
                    );
                }

                return livro;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<
            Livro
        > Update(
            int id,
            LivroDto data
        )
        {
            try
            {
                var livro =
                    await FindById(
                        id
                    );

                var categoriaExiste =
                    await _context
                    .Categorias
                    .AnyAsync(
                        x =>
                        x.Id
                        ==
                        data.CategoriaId
                    );

                if (
                    !categoriaExiste
                )
                {
                    throw new ErrorServiceException(
                        "Categoria não encontrada",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                $"Categoria #{data.CategoriaId} não encontrada"
                            }
                        )
                    );
                }


                _mapper.Map(
                    data,
                    livro
                );

                _context
                    .Livros
                    .Update(
                        livro
                    );

                await _context
                    .SaveChangesAsync();

                return livro;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<
            Livro
        > AddAutores(
            int id,
            LivroAutoresDto autores
        )
        {
            try
            {
                var livro =
                    await FindById(
                        id
                    );

                var lista =
                    await _context
                    .Autores
                    .Where(
                        x =>
                        autores
                        .Ids
                        .Contains(
                            x.Id
                        )
                    )
                    .ToListAsync();


                if (
                    lista.Count
                    ==
                    0
                )
                {
                    throw new ErrorServiceException(
                        "Autores não encontrados",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                "Nenhum autor encontrado"
                            }
                        )
                    );
                }


                foreach (
                    Autor autor
                    in lista
                )
                {
                    if (
                        !livro
                        .Autores!
                        .Any(
                            x =>
                            x.Id
                            ==
                            autor.Id
                        )
                    )
                    {
                        livro
                        .Autores!
                        .Add(
                            autor
                        );
                    }
                }


                await _context
                    .SaveChangesAsync();

                return livro;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task
        Remove(
            int id
        )
        {
            try
            {
                var livro =
                    await FindById(
                        id
                    );

                _context
                    .Livros
                    .Remove(
                        livro
                    );

                await _context
                    .SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}