using AutoMapper;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.FullName,
                o => o.MapFrom(s => $"{s.Person.FirstName} {s.Person.LastName}"))
            .ForMember(d => d.IsActive,
                o => o.MapFrom(s => s.LockoutEnd == null || s.LockoutEnd <= DateTimeOffset.UtcNow))
            .ForMember(d => d.Role,
                o => o.Ignore()); // el rol se asigna en el UseCase via GetRolesAsync
    }
}
