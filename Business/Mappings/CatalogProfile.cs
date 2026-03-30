using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;

namespace Business.Mappings;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<CreateCatalogDto, Data.Entities.Catalog>();
        CreateMap<UpdateCatalogDto, Data.Entities.Catalog>();
        CreateMap<Data.Entities.Catalog, CatalogDto>();
    }
}