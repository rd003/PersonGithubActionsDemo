using Microsoft.AspNetCore.Mvc;
using PersonGithubActionsDemo.Api.DTOS;
using PersonGithubActionsDemo.Api.Extensions;
using PersonGithubActionsDemo.Api.Services;

namespace PersonGithubActionsDemo.Api.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(IPersonService personService, ILogger<PeopleController> logger)
    {
        _personService = personService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        try
        {
            var people = (await _personService.GetPeopleAsync()).Select(p => p.ToPersonReadDto()).ToList();
            return Ok(people);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}", Name = "GetPerson")]
    public async Task<IActionResult> GetPerson(int id)
    {
        try
        {
            PersonReadDTO? person = (await _personService.GetPersonAsync(id))?.ToPersonReadDto();
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}