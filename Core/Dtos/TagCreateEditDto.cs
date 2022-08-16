using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class TagCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
    }
}