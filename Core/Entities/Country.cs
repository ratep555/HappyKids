using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Country : BaseEntity
    {
        [Required]
        [MaxLength(70)]
        public string Name { get; set; }
    }
}