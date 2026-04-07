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
            .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : null))
            .ForMember(dest => dest.DocenteNombre, opt => opt.MapFrom(src => src.Docente != null && src.Docente.Usuario != null && src.Docente.Usuario.Person != null
                ? $"{src.Docente.Usuario.Person.FirstName} {src.Docente.Usuario.Person.LastName}"
                : null));
    }
}

