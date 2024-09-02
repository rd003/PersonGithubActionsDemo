using Microsoft.AspNetCore.Mvc;
using PersonGithubActionsDemo.Api.Domain;
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        try
        {
            Person? person = await _personService.GetPersonAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            await _personService.DeletePersonAsync(person);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPerson(PersonCreateDTO person)
    {
        try
        {
            Person createdPerson = await _personService.AddPersonAsync(person.ToPerson());
            return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.Id }, createdPerson.ToPersonReadDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonUpdateDTO personToUpdate)
    {
        try
        {
            if (id != personToUpdate.Id)
            {
                return BadRequest("Id mismatch");
            }
            Person? person = await _personService.GetPersonAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            await _personService.UpdatePersonAsync(personToUpdate.ToPerson());
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

}