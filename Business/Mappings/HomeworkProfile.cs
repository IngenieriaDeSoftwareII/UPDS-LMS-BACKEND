using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class HomeworkProfile : Profile
{
    public HomeworkProfile()
    {
        CreateMap<CreateHomeworkDto, Homework>();
        CreateMap<UpdateHomeworkDto, Homework>();
        CreateMap<Homework, HomeworkDto>();

        CreateMap<HomeworkSubmission, HomeworkSubmissionDto>()
            .ForMember(dest => dest.EstudianteNombre, 
                opt => opt.MapFrom(src => src.Usuario != null 
                    ? $"{src.Usuario.FirstName} {src.Usuario.LastName} {src.Usuario.MotherLastName}" 
                    : "Desconocido"));
    }
}