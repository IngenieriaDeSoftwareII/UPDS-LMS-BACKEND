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
            .ForMember(dest => dest.UserFullName,
                       opt => opt.MapFrom(src => src.User != null && src.User.Person != null
                       ? $"{src.User.Person.FirstName} {src.User.Person.LastName}" : null));
    }
}