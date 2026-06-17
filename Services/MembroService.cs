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
    public class MembroService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public MembroService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context =
                context;

            _mapper =
                mapper;
        }

        public async Task<
            Membro
        > Create(
            MembroDto data
        )
        {
            try
            {
                var membro =
                    _mapper
                    .Map<Membro>(
                        data
                    );

                await _context
                    .Membros
                    .AddAsync(
                        membro
                    );

                await _context
                    .SaveChangesAsync();

                return membro;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<
            Membro
        > FindById(
            int id
        )
        {
            try
            {
                var membro =
                    await _context
                    .Membros
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        id
                    );

                if (membro is null)
                {
                    throw new ErrorServiceException(
                        "Membro não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                $"Membro #{id} não encontrado"
                            }
                        )
                    );
                }

                return membro;
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
                var membro =
                    await FindById(id);

                _context
                    .Membros
                    .Remove(
                        membro
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