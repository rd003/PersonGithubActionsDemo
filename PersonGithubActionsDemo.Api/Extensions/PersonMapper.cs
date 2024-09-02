using System;
using PersonGithubActionsDemo.Api.Domain;
using PersonGithubActionsDemo.Api.DTOS;

namespace PersonGithubActionsDemo.Api.Extensions;

public static class PersonMapper
{
    public static Person ToPerson(this PersonCreateDTO model)
    {
        return new Person
        {
            Name = model.Name,
            Email = model.Email
        };
    }

    public static Person ToPerson(this PersonUpdateDTO model)
    {
        return new Person
        {
            Id = model.Id,
            Name = model.Name,
            Email = model.Email
        };
    }

    public static PersonCreateDTO ToPersonCreateDto(this Person person)
    {
        return new PersonCreateDTO(person.Name, person.Email);
    }

    public static PersonUpdateDTO ToPersonUpdateDto(this Person person)
    {
        return new PersonUpdateDTO(person.Id, person.Name, person.Email);
    }

    public static PersonReadDTO ToPersonReadDto(this Person person)
    {
        return new PersonReadDTO(person.Id, person.Name, person.Email);
    }
}
