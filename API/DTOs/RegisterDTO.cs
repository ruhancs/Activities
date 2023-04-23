using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage ="password must have uppercase, lowercase and numbers")]
        public string Password { get; set; }
        
        [Required]
        public string DisplayName { get; set; }
        
        [Required]
        public string UserName { get; set; }
    }
}