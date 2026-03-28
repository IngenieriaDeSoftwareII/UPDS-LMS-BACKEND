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
            .ForMember(dest => dest.CursoId, opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.Orden, opt => opt.MapFrom(src => src.Order));

        CreateMap<Lesson, LessonDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titulo))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CursoId))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Orden));
    }
}