using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CreateCategoriaDto, Categoria>();
        CreateMap<UpdateCategoriaDto, Categoria>();
        CreateMap<Categoria, CategoriaDto>();
    }
}
