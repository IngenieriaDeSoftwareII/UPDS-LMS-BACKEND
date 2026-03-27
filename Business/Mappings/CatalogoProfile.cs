using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class CatalogoProfile : Profile
{
    public CatalogoProfile()
    {
        CreateMap<CreateCatalogoDto, Catalogo>();
        CreateMap<UpdateCatalogoDto, Catalogo>();
        CreateMap<Catalogo, CatalogoDto>();
    }
}