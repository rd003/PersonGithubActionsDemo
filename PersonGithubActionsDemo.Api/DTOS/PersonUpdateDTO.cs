using System.ComponentModel.DataAnnotations;

namespace PersonGithubActionsDemo.Api.DTOS;

public class PersonUpdateDTO
{
    public int Id { get; set; }

    [Required]
    [Range(2, 20)]
    public string Name { get; }

    [Range(4, 20)]
    [EmailAddress]
    public string Email { get; }


    public PersonUpdateDTO(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}
