using System.ComponentModel.DataAnnotations;

namespace PersonGithubActionsDemo.Api.DTOS;

public class PersonUpdateDTO
{
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    [MinLength(1)]
    [MaxLength(20)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;


    public PersonUpdateDTO(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}
