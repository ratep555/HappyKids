using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class BranchCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Street { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }
        
        [Range(-90, 90)]
        public double Latitude { get; set; }
        
        [Range(-180, 180)]
        public double Longitude { get; set; } 

        [Required, MaxLength(100)]
        public string WorkingHours { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Phone { get; set; }
        public int CountryId { get; set; }
    }
}