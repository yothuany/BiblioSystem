using AutoMapper;
using BiblioSystem.Dtos.Autor;
using BiblioSystem.Dtos.Categoria;
using BiblioSystem.Dtos.Emprestimo;
using BiblioSystem.Dtos.Exemplar;
using BiblioSystem.Dtos.Livro;
using BiblioSystem.Dtos.Membro;
using BiblioSystem.Dtos.Reserva;
using BiblioSystem.Models;

namespace BiblioSystem.Profiles;

public class LivroProfile : Profile
{
    public LivroProfile()
    {
        // Autor
        CreateMap<Autor, AutorResponseDto>();
        CreateMap<AutorCreateDto, Autor>();
        CreateMap<AutorUpdateDto, Autor>();

        // Categoria
        CreateMap<Categoria, CategoriaResponseDto>();
        CreateMap<CategoriaCreateDto, Categoria>();
        CreateMap<CategoriaUpdateDto, Categoria>();

        // Membro
        CreateMap<Membro, MembroResponseDto>();
        CreateMap<MembroCreateDto, Membro>();
        CreateMap<MembroUpdateDto, Membro>();

        // Exemplar
        CreateMap<Exemplar, ExemplarResponseDto>()
            .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.Livro.Titulo));
        CreateMap<ExemplarCreateDto, Exemplar>();
        CreateMap<ExemplarUpdateDto, Exemplar>();

        // Livro
        CreateMap<Livro, LivroResponseDto>()
            .ForMember(dest => dest.Autores,
                opt => opt.MapFrom(src => src.LivroAutores.Select(la => la.Autor.Nome).ToList()))
            .ForMember(dest => dest.Categorias,
                opt => opt.MapFrom(src => src.LiveCategoria.Select(lc => lc.Categoria.Nome).ToList()));

        // Emprestimo
        CreateMap<Emprestimo, EmprestimoResponseDto>()
            .ForMember(dest => dest.NomeMembro, opt => opt.MapFrom(src => src.Membro.Nome))
            .ForMember(dest => dest.CodigoExemplar, opt => opt.MapFrom(src => src.Exemplar.Codigo));
        CreateMap<EmprestimoCreateDto, Emprestimo>()
            .ForMember(dest => dest.DataEmprestimo, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.Today)));

        // Reserva
        CreateMap<Reserva, ReservaResponseDto>()
            .ForMember(dest => dest.NomeMembro, opt => opt.MapFrom(src => src.Membro.Nome))
            .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.Livro.Titulo));
        CreateMap<ReservaCreateDto, Reserva>();
    }
}
