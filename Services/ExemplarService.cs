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
    public class ExemplarService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public ExemplarService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<
            PaginatedResponse<
                ExemplarResponseDto
            >
        > FindAllV2(
            ExemplarFilter filter
        )
        {
            try
            {
                var query =
                    _context
                    .Exemplares
                    .AsQueryable();

                if (filter.Status is not null)
                {
                    query =
                        query.Where(
                            x =>
                            x.Status
                            ==
                            filter.Status
                        );
                }

                if (filter.LivroId is not null)
                {
                    query =
                        query.Where(
                            x =>
                            x.LivroId
                            ==
                            filter.LivroId
                        );
                }

                return await
                    Paginate<Exemplar>
                    .Set<
                        ExemplarResponseDto
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
            Exemplar
        > Create(
            ExemplarDto data
        )
        {
            try
            {
                var livro =
                    await _context
                    .Livros
                    .AnyAsync(
                        x =>
                        x.Id
                        ==
                        data.LivroId
                    );

                if (!livro)
                {
                    throw new ErrorServiceException(
                        "Livro não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                "Livro inválido"
                            }
                        )
                    );
                }

                var exemplar =
                    _mapper
                    .Map<Exemplar>(
                        data
                    );

                await _context
                    .Exemplares
                    .AddAsync(
                        exemplar
                    );

                await _context
                    .SaveChangesAsync();

                return exemplar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<
            Exemplar
        > FindById(
            int id
        )
        {
            try
            {
                var exemplar =
                    await _context
                    .Exemplares
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        id
                    );

                if (exemplar is null)
                {
                    throw new ErrorServiceException(
                        "Exemplar não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                $"Exemplar #{id} não encontrado"
                            }
                        )
                    );
                }

                return exemplar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<
            Exemplar
        > UpdateStatus(
            int id,
            string status
        )
        {
            try
            {
                var exemplar =
                    await FindById(id);

                exemplar.Status =
                    status;

                await _context
                    .SaveChangesAsync();

                return exemplar;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}