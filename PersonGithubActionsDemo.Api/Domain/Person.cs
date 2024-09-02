using System.ComponentModel.DataAnnotations;

namespace PersonGithubActionsDemo.Api.Domain;

public class Person
{
    public int Id { get; set; }

    [Required]
    [Range(2, 20)]
    public string Name { get; set; } = string.Empty;

    [Range(4, 20)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public Person()
    {

    }

    public Person(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}
