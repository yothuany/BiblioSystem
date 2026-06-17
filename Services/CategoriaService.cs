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
    public class CategoriaService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public CategoriaService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<
            ICollection<
                CategoriaResponseDto
            >
        > FindAll()
        {
            try
            {
                return await _context
                    .Categorias
                    .ProjectTo<
                        CategoriaResponseDto
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
            Categoria
        > Create(
            CategoriaDto data
        )
        {
            try
            {
                var categoria =
                    _mapper
                    .Map<Categoria>(
                        data
                    );

                await _context
                    .Categorias
                    .AddAsync(
                        categoria
                    );

                await _context
                    .SaveChangesAsync();

                return categoria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<
            Categoria
        > FindById(
            int id
        )
        {
            try
            {
                var categoria =
                    await _context
                    .Categorias
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        id
                    );

                if (
                    categoria
                    is null
                )
                {
                    throw new ErrorServiceException(
                        "Categoria não encontrada",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                $"Categoria #{id} não encontrada"
                            }
                        )
                    );
                }

                return categoria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<
            Categoria
        > Update(
            int id,
            CategoriaDto data
        )
        {
            try
            {
                var categoria =
                    await FindById(
                        id
                    );

                _mapper.Map(
                    data,
                    categoria
                );

                _context
                    .Categorias
                    .Update(
                        categoria
                    );

                await _context
                    .SaveChangesAsync();

                return categoria;
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
                var categoria =
                    await FindById(
                        id
                    );

                _context
                    .Categorias
                    .Remove(
                        categoria
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