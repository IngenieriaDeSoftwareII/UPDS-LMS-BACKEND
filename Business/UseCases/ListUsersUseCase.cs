using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ListUsersUseCase(IUserRepository userRepository, IMapper mapper)
{
    public async Task<IEnumerable<UserDto>> ExecuteAsync(string? search = null)
    {
        var users = await userRepository.GetAllAsync(search);

        var result = new List<UserDto>();
        foreach (var user in users)
        {
            var dto = mapper.Map<UserDto>(user);
            var roles = await userRepository.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault() ?? string.Empty;
            result.Add(dto);
        }

        return result;
    }
}
