using System.Text.Json;
using PersonGithubActionsDemo.Api.DTOS;

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


        //Act 
        var response = await _client.GetAsync("api/People");

        // Act
        response.EnsureSuccessStatusCode(); // Status Code 2xx
        var reponseString = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var people = JsonSerializer.Deserialize<List<PersonReadDTO>>(reponseString,options);

        // Assert
        Assert.NotNull(people);
        Assert.NotEmpty(people);
    }

    [Fact]
    public async Task GetPerson_ReturnsOk_WhenPersonExists()
    {
        // Arrange: 

        // Act 
        var response = await _client.GetAsync("api/People/1");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 2xx
        // TODO: Copy response string
        var responseString = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var person = JsonSerializer.Deserialize<PersonReadDTO>(responseString,options);
        Assert.NotNull(person);
        Assert.Equal(1, person.Id);
        Assert.Equal("John", person.Name);
        Assert.Equal("john@example.com", person.Email);
    }

}
