using Microsoft.AspNetCore.Mvc;

namespace PersonGithubActionsDemo.Api.Controllers;

[ApiController]
[Route("/api/[Controller]/{id}")]
public class PeopleController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        return Ok();
    }
}