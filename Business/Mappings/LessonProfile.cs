using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<CreateLessonDto, Lesson>()
            .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ModuloId, opt => opt.MapFrom(src => src.ModuleId))
            .ForMember(dest => dest.Orden, opt => opt.MapFrom(src => src.Order));

        CreateMap<Lesson, LessonDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titulo))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.ModuleId, opt => opt.MapFrom(src => src.ModuloId))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Orden));
    }
}