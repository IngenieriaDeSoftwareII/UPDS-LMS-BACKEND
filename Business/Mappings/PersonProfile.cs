using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<CreatePersonDto, Person>();
        CreateMap<Person, PersonDto>();
    }
}
