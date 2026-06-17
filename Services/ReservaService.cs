using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services
{
    public class ReservaService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public ReservaService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;

            _mapper = mapper;
        }


        public async Task<
            Reserva
        > Create(
            ReservaDto data
        )
        {
            try
            {
                var reserva =
                    _mapper
                    .Map<
                        Reserva
                    >(
                        data
                    );


                reserva.Status =
                    "ativa";


                await _context
                    .Reservas
                    .AddAsync(
                        reserva
                    );

                await _context
                    .SaveChangesAsync();

                return reserva;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<
            Reserva
        > Cancelar(
            int id
        )
        {
            try
            {
                var reserva =
                    await _context
                    .Reservas
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        id
                    );

                if (
                    reserva
                    is null
                )
                {
                    throw new ErrorServiceException(
                        "Reserva não encontrada",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                "Reserva inválida"
                            }
                        )
                    );
                }


                reserva.Status =
                    "cancelada";

                await _context
                    .SaveChangesAsync();

                return reserva;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}