using AutoMapper;
using BiblioSystem.Dtos;
using BiblioSystem.Dtos.Responses;
using BiblioSystem.Models;

namespace BiblioSystem.Profiles
{
    public class BibliotecaProfile : Profile
    {
        public BibliotecaProfile()
        {
            /*
             * DTO ? MODEL
             */

            CreateMap<LivroDto, Livro>();

            CreateMap<AutorDto, Autor>();

            CreateMap<CategoriaDto, Categoria>();

            CreateMap<ExemplarDto, Exemplar>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => "disponivel")
                );

            CreateMap<MembroDto, Membro>()
                .ForMember(
                    dest => dest.DataCadastro,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now))
                );

            CreateMap<UsuarioDto, Usuario>();

            CreateMap<EmprestimoDto, Emprestimo>()
                .ForMember(
                    dest => dest.DataEmprestimo,
                    opt => opt.MapFrom(
                        src => DateOnly.FromDateTime(DateTime.Now)
                    )
                )
                .ForMember(
                    dest => dest.Multa,
                    opt => opt.MapFrom(src => 0)
                );

                CreateMap<
                    ReservaDto,
                    Reserva
                >()

                .ForMember(
                    dest => dest.DataReserva,

                    opt =>
                    opt.MapFrom(
                        src =>
                        DateTime.Now
                    )
                )

                .ForMember(
                    dest => dest.Status,

                    opt =>
                    opt.MapFrom(
                        src =>
                        "ativa"
                    )
                );


                CreateMap<
                    Reserva,
                    ReservaResponseDto
                >();
           


    /*
     * MODEL ? RESPONSE DTO
     */

    CreateMap<Categoria, CategoriaResponseDto>();

            CreateMap<Autor, AutorResponseDto>();

            CreateMap<Exemplar, ExemplarResponseDto>();

            CreateMap<Membro, MembroResponseDto>();

            CreateMap<Usuario, UsuarioResponseDto>();

            CreateMap<Emprestimo, EmprestimoResponseDto>();

            CreateMap<Reserva, ReservaResponseDto>();

            CreateMap<Livro, LivroResponseDto>();
        }
    }
}