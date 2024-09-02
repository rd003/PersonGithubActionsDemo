using Microsoft.EntityFrameworkCore;
using PersonGithubActionsDemo.Api.Domain;

namespace PersonGithubActionsDemo.Api.Data;

public class PersonContext : DbContext
{
    public PersonContext(DbContextOptions<PersonContext> options) : base(options)
    {

    }

    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Person>().ToTable("Person");

        modelBuilder.Entity<Person>().HasData(new List<Person>{
           new () {Id=1,Name="John",Email="john@example.com"},
           new () {Id=2,Name="Jim",Email="jim@example.com"}
        });
    }
}
