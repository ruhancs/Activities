using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser: IdentityUser
    {
        //propiedades adicionais ao IdentityUser
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }
}