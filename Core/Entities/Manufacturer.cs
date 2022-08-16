using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Manufacturer : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}