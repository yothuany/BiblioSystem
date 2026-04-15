using AutoMapper;
using BiblioSystem.Dtos;
using BiblioSystem.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BiblioSystem.Profiles
{
    public class LivroProfile : Profile
    {
        public LivroProfile()
        {
            CreateMap<LivroDto, Livro>()
                .ForMember(
                    dest => dest.Situacao,
                    opt => opt.MapFrom(src => "disponivel")
                );

            CreateMap<LivroUpdateDto, Livro>()
                .ForMember(
                    dest => dest.DataAtualizacao,
                    opt => opt.MapFrom(
                        src => src.DataAtualizacao.ToDateTime(TimeOnly.FromDateTime(DateTime.Now))
                    )
                );
        }
    }
}