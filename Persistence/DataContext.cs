using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// migraçoes
//dotnet ef migrations add InitialCreate -s API -p Persistence
//dotnet ef migrations add IdentityAdded -p Persistence -s API

//criaçao do database feita em program

namespace Persistence
{
    //habilitar o DataContext em program como service
     //IdentityDbContext<AppUser>ja habilita a entidade AppUser
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // Activities representa o nome da tabela no db
        public DbSet<Domain.Activity> Activities { get; set; }
    }
}