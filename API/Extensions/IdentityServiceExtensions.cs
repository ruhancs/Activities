using System.Text;
using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(
            this IServiceCollection service,
            IConfiguration config
        )
        {
            service.AddIdentityCore<AppUser>(opt => 
            {
                opt.Password.RequireNonAlphanumeric= false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => 
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,//para o token expirar
                        ClockSkew = TimeSpan.Zero//invalidar no exato momento que expira
                    };

                    // configuraçao para utilizar autenticaçao com signalR
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context => 
                        {
                            var accesssToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if(!string.IsNullOrEmpty(accesssToken) && (path.StartsWithSegments("/chat")))
                            {
                                context.Token = accesssToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            service.AddAuthorization(opt => {
                opt.AddPolicy("IsActivityHost", policy => {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });

            service.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

            service.AddScoped<TokenService>();

            return service;
        }
    }
}