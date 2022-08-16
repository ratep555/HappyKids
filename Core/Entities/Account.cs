using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Account : BaseEntity
    {
        [Required]
		[MaxLength(255)]
        public string BankName { get; set; }

        [Required]
		[MaxLength(50)]
        public string IBAN { get; set; }
    }
}