using API.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//adicionado a extensao de serviços criados em ApplicationServiceExtension
//que configura os services utilizados em application
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//criar database
// using para quando terminar de utilizar o scope se destroi automaticamente limpa memoria
//scope criado para ter acesso a Services
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<Persistence.DataContext>();
    await context.Database.MigrateAsync();

    //adicionar os dados criados em seed ao db
    await Persistence.Seed.SeedData(context);
}
catch (Exception ex)
{
    
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An eroor occured during migration");
}

app.Run();
