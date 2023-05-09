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
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // configurando primary key da tabela de join de activity e user
            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserId, aa.ActivityId}));
        
            //relacao entre user e activity
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

            //relacao entre activity e user
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId);
        }
    }
}