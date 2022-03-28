using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Identity
{
    public class RegisterDto
    {
        [Required, MinLength(2), MaxLength(20)]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}