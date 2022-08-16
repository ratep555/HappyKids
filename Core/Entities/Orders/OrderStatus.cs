using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Orders
{
    public class OrderStatus : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}