using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class MessageCreateEditDto
    {
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(255)]
        public string FirstLastName { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Phone { get; set; }
        

        [Required, MinLength(10), MaxLength(2000)]
        public string MessageContent { get; set; }
        public bool IsReplied { get; set; }
    }
}