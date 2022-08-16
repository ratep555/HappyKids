using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class OrderStatusCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
    }
}