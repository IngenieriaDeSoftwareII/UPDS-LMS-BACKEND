using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : null));
    }
}
