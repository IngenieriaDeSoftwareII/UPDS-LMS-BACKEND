using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases;

public class ListPersonsUseCase(IPersonRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<PersonDto>> ExecuteAsync()
    {
        var persons = await repository.GetAllAsync();
        return mapper.Map<IEnumerable<PersonDto>>(persons);
    }
}
