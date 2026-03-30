using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<CreateTeacherDto, Teacher>();
        CreateMap<UpdateTeacherDto, Teacher>();
        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.NombreCompleto,
                       opt => opt.MapFrom(src => src.Usuario != null && src.Usuario.Person != null
                       ? $"{src.Usuario.Person.FirstName} {src.Usuario.Person.LastName}" : null));
    }
}