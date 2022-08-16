using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Warehouse : BaseEntity
    {
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Required]
        [MaxLength(100)]      
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Street { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

    }
}