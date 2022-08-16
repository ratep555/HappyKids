using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Core.Entities
{
    public class Branch : BaseEntity
    {
        public int CountryId { get; set; }
        public Country Country { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(120)]
        public string Street { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(100)]
        public string WorkingHours { get; set; }
        public Point Location { get; set; }
    }
}