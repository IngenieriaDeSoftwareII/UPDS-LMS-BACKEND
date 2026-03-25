using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;
namespace Business.Mappings;

public class InscriptionProfile : Profile
{
    public InscriptionProfile()
    {
        CreateMap<CreateInscriptionDto, Inscription>();
        CreateMap<InscriptionByStudentDto, Inscription>();
        CreateMap<Inscription, InscriptionDto>()
            .ForMember(d => d.Estado,
                o => o.MapFrom(s => s.Estado.ToString().ToLowerInvariant()));
    }
}