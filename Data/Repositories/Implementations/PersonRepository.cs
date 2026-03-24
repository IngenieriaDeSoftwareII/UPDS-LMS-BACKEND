using Data.Context;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class PersonRepository(AppDbContext context) : IPersonRepository
{
    public async Task<Person> CreateAsync(Person person)
    {
        context.People.Add(person);
        await context.SaveChangesAsync();
        return person;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await context.People.ToListAsync();
    }
}
