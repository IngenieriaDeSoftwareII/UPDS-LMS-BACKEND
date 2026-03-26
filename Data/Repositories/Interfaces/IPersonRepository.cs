using Data.Entities;

namespace Data.Repositories.Interfaces;

public interface IPersonRepository
{
    Task<Person> CreateAsync(Person person);
    Task<Person?> GetByIdAsync(int id);
    Task<IEnumerable<Person>> GetAllAsync();
}
