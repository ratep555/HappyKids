using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Dtos.BirthdayOrdersDtos
{
    public class KidActivityCreateEditDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
        public string VideoClip { get; set; }
    }
}