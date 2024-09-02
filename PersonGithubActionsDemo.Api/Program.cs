using Microsoft.EntityFrameworkCore;
using PersonGithubActionsDemo.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PersonContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("default")));

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
