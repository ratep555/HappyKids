using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.WarehousesDtos
{
    public class WarehouseCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Street { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }
        public int CountryId { get; set; }
    }
}