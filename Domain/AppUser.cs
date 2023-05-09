using Microsoft.AspNetCore.Identity;

// comando de migraçao 
//  dotnet ef migrations add ActivityAttendee -p Persistence -s API

namespace Domain
{
    public class AppUser: IdentityUser
    {
        //propiedades adicionais ao IdentityUser
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public ICollection<ActivityAttendee> Activities { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}