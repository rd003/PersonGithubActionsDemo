using System.ComponentModel.DataAnnotations;

namespace PersonGithubActionsDemo.Api.DTOS;

public class PersonCreateDTO
{
    [Required]
    [MinLength(4)]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    [MinLength(1)]
    [MaxLength(20)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;


    public PersonCreateDTO(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
