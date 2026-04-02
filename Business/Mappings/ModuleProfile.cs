using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class ModuleProfile : Profile
{
    public ModuleProfile()
    {
        CreateMap<CreateModuleDto, Module>();
        CreateMap<UpdateModuleDto, Module>();
        CreateMap<Module, ModuleDto>();
    }
}