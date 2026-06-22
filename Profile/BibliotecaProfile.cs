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
            CreateMap<LivroDto, Livro>();
            CreateMap<AutorDto, Autor>();
            CreateMap<CategoriaDto, Categoria>();
            CreateMap<UsuarioDto, Usuario>();
            CreateMap<MembroDto, Membro>();

            CreateMap<ExemplarDto, Exemplar>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Disponível"));

            CreateMap<ReservaDto, Reserva>()
                .ForMember(dest => dest.DataReserva, opt => opt.MapFrom(src => DateTime.Now.Date))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Ativa"));

            CreateMap<EmprestimoDto, Emprestimo>()
                .ForMember(dest => dest.DataEmprestimo, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Ativo"))
                .ForMember(dest => dest.ValorMulta, opt => opt.MapFrom(src => 0.00m));

            CreateMap<Livro, LivroResponseDto>();
            CreateMap<Autor, AutorResponseDto>();
            CreateMap<Categoria, CategoriaResponseDto>();
            CreateMap<Membro, MembroResponseDto>();
            CreateMap<Usuario, UsuarioResponseDto>();

            CreateMap<Emprestimo, EmprestimoResponseDto>()
                .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.Exemplar != null && src.Exemplar.Livro != null ? src.Exemplar.Livro.Titulo : null))
                .ForMember(dest => dest.NomeMembro, opt => opt.MapFrom(src => src.Membro != null ? src.Membro.Nome : null))
                .ForMember(dest => dest.CodigoExemplar, opt => opt.MapFrom(src => src.Exemplar != null ? src.Exemplar.Codigo : null));


            CreateMap<Exemplar, ExemplarResponseDto>()
                .ForMember(dest => dest.LivroTitulo, opt => opt.MapFrom(src => src.Livro != null ? src.Livro.Titulo : null));

            CreateMap<Reserva, ReservaResponseDto>()
                .ForMember(dest => dest.LivroTitulo, opt => opt.MapFrom(src => src.Livro != null ? src.Livro.Titulo : null))
                .ForMember(dest => dest.MembroNome, opt => opt.MapFrom(src => src.Membro != null ? src.Membro.Nome : null));
        }
    }
}