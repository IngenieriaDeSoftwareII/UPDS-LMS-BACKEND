using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class DocenteProfile : Profile
{
    public DocenteProfile()
    {
        CreateMap<CreateDocenteDto, Docente>();
        CreateMap<UpdateDocenteDto, Docente>();
        CreateMap<Docente, DocenteDto>();
    }
}
