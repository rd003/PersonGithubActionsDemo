using Microsoft.EntityFrameworkCore;
using PersonGithubActionsDemo.Api.Data;
using PersonGithubActionsDemo.Api.Domain;

namespace PersonGithubActionsDemo.Api.Services;

public class PersonService : IPersonService
{
    private readonly PersonContext _context;
    public PersonService(PersonContext context)
    {
        _context = context;
    }
    public async Task<Person> AddPersonAsync(Person person)
    {
        _context.People.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task<Person> UpdatePersonAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task DeletePersonAsync(Person person)
    {
        _context.People.Remove(person);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Person>> GetPeopleAsync() =>
      await _context.People.AsNoTracking().ToListAsync();

    public async Task<Person?> GetPersonAsync(int id) =>
      await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

}
