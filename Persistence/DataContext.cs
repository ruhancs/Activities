using Microsoft.EntityFrameworkCore;

// fazer a primeira migraçao do db
//dotnet ef migrations add InitialCreate -s API -p Persistence

//criaçao do database feita em program

namespace Persistence
{
    //habilitar o DataContext em program como service 
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // Activities representa o nome da tabela no db
        public DbSet<Domain.Activity> Activities { get; set; }
    }
}