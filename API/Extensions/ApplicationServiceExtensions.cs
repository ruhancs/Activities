using Application.Activities;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using FluentValidation.AspNetCore;
using FluentValidation;
using Application.interfaces;
using Infrastructure.Security;
using Infrastructure.Photos;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // adicionar DataContext para ser usado
            services.AddDbContext<DataContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                
            });

            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()//police para signalR
                        .WithOrigins("http://localhost:3000");
                });
            });

            //registro do mediator para querys no db
            services.AddMediatR(typeof(ListUseCase.Handler));

            //registro do automapper que esta em application Core MappingProfiles
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            //habilitar validaçoes das requests
            services.AddFluentValidationAutoValidation();
            //onde sera aplicado as validaçoes
            services.AddValidatorsFromAssemblyContaining<CreateUseCase>();

            //httpcontext para pegar o token
            services.AddHttpContextAccessor();

            services.AddScoped<IUserAccessorRepository, UserAccessorRepository>();

            // declarar o serviço de upload que esta em infrastructure
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            // cloudinary para upload de imagens
            //inserir variaveis em app.settings no cloudinary
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            
            services.AddSignalR();

            
            return services;
        }
    }
}