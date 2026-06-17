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
    public class AutorService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public AutorService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<
            ICollection<AutorResponseDto>
        > FindAll()
        {
            try
            {
                return await _context
                    .Autores
                    .ProjectTo<
                        AutorResponseDto
                    >(
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
                AutorResponseDto
            >
        > FindAllV2(
            AutorFilter filter
        )
        {
            try
            {
                var query =
                    _context
                    .Autores
                    .AsQueryable();

                if (
                    filter.Search
                    is not null
                )
                {
                    query =
                        query
                        .Where(
                            x =>
                            x.Nome
                            .Contains(
                                filter.Search
                            )
                        );
                }

                return await
                    Paginate<Autor>
                    .Set<
                        AutorResponseDto
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
            Autor
        > Create(
            AutorDto data
        )
        {
            try
            {
                var autor =
                    _mapper
                    .Map<Autor>(
                        data
                    );

                await _context
                    .Autores
                    .AddAsync(
                        autor
                    );

                await _context
                    .SaveChangesAsync();

                return autor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<
            Autor
        > FindById(
            int id
        )
        {
            try
            {
                var autor =
                    await _context
                    .Autores
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        id
                    );

                if (
                    autor
                    is null
                )
                {
                    throw new ErrorServiceException(
                        "Autor não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                $"Autor #{id} não encontrado"
                            }
                        )
                    );
                }

                return autor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<
            Autor
        > Update(
            int id,
            AutorDto data
        )
        {
            try
            {
                var autor =
                    await FindById(
                        id
                    );

                _mapper.Map(
                    data,
                    autor
                );

                _context
                    .Autores
                    .Update(
                        autor
                    );

                await _context
                    .SaveChangesAsync();

                return autor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Remove(
            int id
        )
        {
            try
            {
                var autor =
                    await FindById(
                        id
                    );

                _context
                    .Autores
                    .Remove(
                        autor
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