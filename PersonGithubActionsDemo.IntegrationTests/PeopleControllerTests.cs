using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PersonGithubActionsDemo.Api.Data;
using PersonGithubActionsDemo.Api.Domain;
using PersonGithubActionsDemo.Api.DTOS;
using PersonGithubActionsDemo.Api.Services;

namespace PersonGithubActionsDemo.IntegrationTests;

public class PeopleControllerTests : IClassFixture<MyWebApplicationFactory<Program>>
{
    private readonly MyWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public PeopleControllerTests(MyWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetPeople_ReturnsOkResponse()
    {
        // Arrange
        await SeedTestDataAsync();


        //Act 
        var response = await _client.GetAsync("api/People");

        // Act
        response.EnsureSuccessStatusCode(); // Status Code 2xx
        var reponseString = await response.Content.ReadAsStringAsync();
        var people = JsonSerializer.Deserialize<List<PersonReadDTO>>(reponseString);
        Assert.NotNull(people);
        Assert.NotEmpty(people);
    }

    [Fact]
    public async Task GetPerson_ReturnsOk_WhenPersonExists()
    {
        // Arrange: 
        await SeedTestDataAsync();

        // Act 
        var response = await _client.GetAsync("api/People/1");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 2xx
        var responseString = await response.Content.ReadAsStringAsync();
        var person = JsonSerializer.Deserialize<PersonReadDTO>(responseString);
        Assert.NotNull(person);
        Assert.Equal(1, person.Id);
        Assert.Equal("John", person.Name);
        Assert.Equal("john@example.com", person.Email);
    }

    private async Task SeedTestDataAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PersonContext>();

        // Ensure the database is created and clean before seeding
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed data
        var people = new List<Person>
            {
                new Person { Id = 1, Name = "John", Email = "john@example.com" },
                new Person { Id = 2, Name = "Jane", Email = "jane@example.com" }
            };

        if (context.People.Any() == false)
        {
            context.People.AddRange(people);
            await context.SaveChangesAsync();
        }
    }


}
