using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class ShippingOptionCreateEitDto
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(50)]
        public string TransitDays { get; set; }
        public decimal Price { get; set; }
    }
}