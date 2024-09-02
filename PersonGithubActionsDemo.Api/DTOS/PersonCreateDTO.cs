using System.ComponentModel.DataAnnotations;

namespace PersonGithubActionsDemo.Api.DTOS;

public class PersonCreateDTO
{
    [Required]
    [Range(2, 20)]
    public string Name { get; } = string.Empty;

    [Range(4, 20)]
    [EmailAddress]
    public string Email { get; } = string.Empty;


    public PersonCreateDTO(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
