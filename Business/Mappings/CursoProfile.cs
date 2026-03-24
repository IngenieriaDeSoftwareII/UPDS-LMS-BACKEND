using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class CursoProfile : Profile
{
    public CursoProfile()
    {
        CreateMap<CreateCursoDto, Curso>();
        CreateMap<UpdateCursoDto, Curso>();
        CreateMap<Curso, CursoDto>()
            .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : null));
    }
}
