
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PersonGithubActionsDemo.Api.Controllers;
using PersonGithubActionsDemo.Api.Domain;
using PersonGithubActionsDemo.Api.DTOS;
using PersonGithubActionsDemo.Api.Extensions;
using PersonGithubActionsDemo.Api.Services;

namespace PersonGithubActionsDemo.UnitTests;

public class PeopleControllerTests
{
    private readonly IPersonService _personService;
    private readonly ILogger<PeopleController> _logger;
    private readonly PeopleController _controller;
    List<Person> people = new List<Person>
        {
            new Person(1, "John", "john@example.com"),
            new Person(2, "Jim", "jim@example.com"),
            new Person(3, "Rick","rick@example.com")
        };
    public PeopleControllerTests()
    {
        _personService = Substitute.For<IPersonService>();
        _logger = Substitute.For<ILogger<PeopleController>>();
        _controller = new PeopleController(_personService, _logger);
    }


    [Fact]
    public async Task GetPeople_ReturnsOkResult_WithPeopleList()
    {
        // Arrange
        _personService.GetPeopleAsync().Returns(people);

        // Act
        var result = await _controller.GetPeople();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var peopleList = Assert.IsType<List<PersonReadDTO>>(okResult.Value);
        Assert.True(peopleList.Count > 0);
    }

    [Fact]
    public async Task GetPerson_ReturnOKResult_WithPerson()
    {
        // Arrange
        int id = 1;
        var person = people.First(a => a.Id == id);
        _personService.GetPersonAsync(id).Returns(person);

        // Act
        var result = await _controller.GetPerson(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var personResult = Assert.IsType<PersonReadDTO>(okResult.Value);
        Assert.Equal(id, personResult.Id);
    }

    [Fact]
    public async Task GetPerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange 
        int id = 1;

        // Act
        var result = await _controller.GetPerson(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNoContentResult()
    {
        // Arrange
        var id = 1;
        var person = people.First(a => a.Id == id);
        _personService.GetPersonAsync(id).Returns(person);

        // Act
        var result = await _controller.DeletePerson(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNotFoundResult_WhenPersonDoesNotExist()
    {
        // Arrange
        int id = 999; // ID that doesn't exist

        // Act
        var result = await _controller.DeletePerson(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddPerson_ReturnsCreatedAtRouteResult_WhenPersonAddedSuccessfully()
    {
        // Arrange
        PersonCreateDTO personCreateDto = new PersonCreateDTO("John", "john@example.com");
        var person = people.FirstOrDefault(a => a.Name == "John" && a.Email == "john@example.com");
        _personService.AddPersonAsync(Arg.Any<Person>()).Returns(person);

        // Act
        var result = await _controller.AddPerson(personCreateDto);

        // Assert
        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(nameof(_controller.GetPerson), createdAtRouteResult.RouteName);
        Assert.Equal(1, createdAtRouteResult.RouteValues["id"]);

        var personReadDTO = Assert.IsType<PersonReadDTO>(createdAtRouteResult.Value);

        Assert.Equal(person.Id, personReadDTO.Id);
    }

    [Fact]
    public async Task AddPerson_ReturnsStatusCodeResult_WhenServiceThrowsException()
    {
        // Arrange
        var personCreateDTO = new PersonCreateDTO("John", "john@example.com");
        _personService.AddPersonAsync(Arg.Any<Person>()).Throws<Exception>();

        // Act
        var result = await _controller.AddPerson(personCreateDTO);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    // [Fact]
    // public async Task AddPerson_ReturnsStatusCodeResult_WhenPersonValidationFails()
    // {
    //     // Arrange
    //     var personCreateDTO = new PersonCreateDTO(string.Empty, "john@example.com");

    //     // Act
    //     var result = await _controller.AddPerson(personCreateDTO);

    //     // Assert
    //     var objectResult = Assert.IsType<ObjectResult>(result);
    //     Assert.Equal(400, objectResult.StatusCode);
    // }

    [Fact]
    public async Task UpdatePerson_ReturenNoContent_WhenPersonUpdated()
    {
        // Arrange
        int id = 1;
        var personUpdateDto = new PersonUpdateDTO(id, "John", "john@example.com");
        var person = people.First(a => a.Id == personUpdateDto.Id);
        _personService.GetPersonAsync(id).Returns(person);

        // Act
        var result = await _controller.UpdatePerson(id, personUpdateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        int id = 1;
        PersonUpdateDTO personToUpdate = new PersonUpdateDTO(2, "John", "john@example.com");

        // Act
        var result = await _controller.UpdatePerson(id, personToUpdate);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Id mismatch", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsNotFound_WhenPersonNotFound()
    {
        // Arrange
        int id = 1;
        PersonUpdateDTO personToUpdate = new PersonUpdateDTO(id, "John", "john@example.com");
        _personService.GetPersonAsync(Arg.Any<int>()).Returns((Person)null);

        // Act
        var result = await _controller.UpdatePerson(id, personToUpdate);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

}