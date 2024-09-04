using System.Net;
using System.Text;
using System.Text.Json;
using PersonGithubActionsDemo.Api.Domain;
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
        var people = JsonSerializer.Deserialize<List<PersonReadDTO>>(reponseString, options);

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
        var person = JsonSerializer.Deserialize<PersonReadDTO>(responseString, options);
        Assert.NotNull(person);
        Assert.Equal(1, person.Id);
        Assert.Equal("John", person.Name);
        Assert.Equal("john@example.com", person.Email);
    }

    [Fact]
    public async Task GetPerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/People/999"); // record with this id does not exists

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddPerson_ReturnsCreatedAtRoute()
    {
        // Arrange
        var newPerson = new Person(0, "Rick", "rick@example.com");
        var content = new StringContent(JsonSerializer.Serialize(newPerson), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/people", content);
        response.EnsureSuccessStatusCode();  // status code 2xx

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var createdPerson = JsonSerializer.Deserialize<PersonReadDTO>(jsonResponse, options);

        // Assert
        Assert.NotNull(createdPerson);
        Assert.Equal("Rick", createdPerson.Name);
        Assert.Equal("rick@example.com", createdPerson.Email);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange 
        var personToUpdate = new Person(1, "John Doe", "johndoe@example.com");
        var content = new StringContent(JsonSerializer.Serialize(personToUpdate), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/people/1", content);
        response.EnsureSuccessStatusCode();  // status code 2xx

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


    [Fact]
    public async Task UpdatePerson_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange 
        var personToUpdate = new Person(1, "John Doe", "johndoe@example.com");
        var content = new StringContent(JsonSerializer.Serialize(personToUpdate), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/people/2", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var personToUpdate = new Person(999, "Jack", "jack@example.com");
        var content = new StringContent(JsonSerializer.Serialize(personToUpdate), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/people/999", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNoContent_WhenDeletionIsSuccessfull()
    {
        // Arrange

        // Act
        var response = await _client.DeleteAsync("api/people/1");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 2xx
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange

        // Act
        var response = await _client.DeleteAsync("api/people/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
}
