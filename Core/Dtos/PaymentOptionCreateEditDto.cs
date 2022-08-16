using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class PaymentOptionCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Name { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }
    }
}