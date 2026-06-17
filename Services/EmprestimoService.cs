using AutoMapper;
using BiblioSystem.DataContexts;
using BiblioSystem.Dtos;
using BiblioSystem.Exceptions;
using BiblioSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Services
{
    public class EmprestimoService
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public EmprestimoService(
            AppDbContext context,
            IMapper mapper
        )
        {
            _context = context;

            _mapper = mapper;
        }


        public async Task<
            Emprestimo
        > Create(
            EmprestimoDto data
        )
        {
            try
            {
                var membro =
                    await _context
                    .Membros
                    .AnyAsync(
                        x =>
                        x.Id
                        ==
                        data.MembroId
                    );

                if (!membro)
                {
                    throw new ErrorServiceException(
                        "Membro não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                "Membro inválido"
                            }
                        )
                    );
                }


                var exemplar =
                    await _context
                    .Exemplares
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        data.ExemplarId
                    );


                if (
                    exemplar
                    is null
                )
                {
                    throw new ErrorServiceException(
                        "Exemplar não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                "Exemplar inválido"
                            }
                        )
                    );
                }


                if (
                    exemplar.Status
                    !=
                    "disponivel"
                )
                {
                    throw new ErrorServiceException(
                        "Exemplar indisponível",

                        c =>
                        c.Conflict(
                            new
                            {
                                message =
                                "Livro indisponível"
                            }
                        )
                    );
                }


                var emprestimo =
                    _mapper
                    .Map<
                        Emprestimo
                    >(
                        data
                    );

                exemplar.Status =
                    "emprestado";


                await _context
                    .Emprestimos
                    .AddAsync(
                        emprestimo
                    );

                await _context
                    .SaveChangesAsync();

                return emprestimo;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<
            Emprestimo
        > RegistrarDevolucao(
            int id
        )
        {
            try
            {
                var emprestimo =
                    await _context
                    .Emprestimos
                    .Include(
                        x =>
                        x.Exemplar
                    )
                    .FirstOrDefaultAsync(
                        x =>
                        x.Id
                        ==
                        id
                    );

                if (
                    emprestimo
                    is null
                )
                {
                    throw new ErrorServiceException(
                        "Empréstimo não encontrado",

                        c =>
                        c.NotFound(
                            new
                            {
                                message =
                                "Registro inexistente"
                            }
                        )
                    );
                }


                emprestimo.DataDevolucao =
                    DateOnly
                    .FromDateTime(
                        DateTime.Now
                    );

                emprestimo.Exemplar!.Status =
                    "disponivel";


                emprestimo.Multa =
                    CalcularMulta(
                        emprestimo
                    );


                await _context
                    .SaveChangesAsync();

                return emprestimo;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public decimal
        CalcularMulta(
            Emprestimo emprestimo
        )
        {
            if (
                emprestimo.DataDevolucao
                is null
            )
            {
                return 0;
            }


            var atraso =
                (
                    emprestimo
                    .DataDevolucao
                    .Value
                    .ToDateTime(
                        TimeOnly.MinValue
                    )

                    -

                    emprestimo
                    .DataPrevistaDevolucao
                    .ToDateTime(
                        TimeOnly.MinValue
                    )

                )
                .Days;


            if (
                atraso
                <=
                0
            )
            {
                return 0;
            }


            return atraso * 2;
        }
    }
}