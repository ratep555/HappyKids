using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Orders
{
   public class PaymentOption : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}