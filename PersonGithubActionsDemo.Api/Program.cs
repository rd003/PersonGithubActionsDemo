using Microsoft.EntityFrameworkCore;
using PersonGithubActionsDemo.Api.Data;
using PersonGithubActionsDemo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PersonContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("default")));

builder.Services.AddScoped<IPersonService, PersonService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<PersonContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}


app.Run();
