using API.Extensions;
using API.Middleware;
using API.SIgnalIR;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => 
{
    //para indicar que os endpoints requerem authentication
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

//adicionado a extensao de serviços criados em ApplicationServiceExtension
//que configura os services utilizados em application
builder.Services.AddApplicationServices(builder.Configuration);

//criado em extensions IdentityServiceExtensions
//configuraçoes da entidade AppUser e authenticaçao
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

//middleware de erros
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// configuraçao para deploy

app.UseDefaultFiles(); // busca em wwwroot arquivos index.html
app.UseStaticFiles(); // pega o conteudo dentro de wwwroot 

app.MapControllers();

// configuraçao do Hub signalR, nome da classe e a rota
app.MapHub<ChatHub>("/chat");

// buscar as rotas nao conhecidas na API em FallbackController
app.MapFallbackToController("Index", "Fallback");// nome da funçao e FallbackController

//criar database
// using para quando terminar de utilizar o scope se destroi automaticamente limpa memoria
//scope criado para ter acesso a Services
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<Persistence.DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();

    //adicionar os dados criados em seed ao db
    await Persistence.Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An eroor occured during migration");
}

app.Run();
