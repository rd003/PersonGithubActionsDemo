using PersonGithubActionsDemo.Api.Domain;

namespace PersonGithubActionsDemo.Api.Services;

public interface IPersonService
{
    public Task<Person> AddPersonAsync(Person person);
    public Task<Person> UpdatePersonAsync(Person person);
    public Task DeletePersonAsync(Person person);
    public Task<IEnumerable<Person>> GetPeopleAsync();
    public Task<Person?> GetPersonAsync(int id);
}
