using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IPersonRepository
{
    Task<Person> CreateAsync(Person person);
    Task<IEnumerable<Person>> GetAllAsync();
}
