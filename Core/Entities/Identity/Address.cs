using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    public class Address : BaseEntity
    {
        [Required]
		[MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
		[MaxLength(100)]
        public string LastName { get; set; }

        [Required]
		[MaxLength(100)]
        public string City { get; set; }

        [Required]
		[MaxLength(100)]
        public string Street { get; set; }
        
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}