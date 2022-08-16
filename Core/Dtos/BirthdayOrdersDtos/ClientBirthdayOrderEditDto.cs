using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.BirthdayOrdersDtos
{
    public class ClientBirthdayOrderEditDto
    {
        public int Id { get; set; }  
        public int BranchId { get; set; }
        public int BirthdayPackageId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ClientName { get; set; }

        [Required]
        [MaxLength(255)]
        public string BirthdayGirlBoyName { get; set; }

        [Required]
        [MaxLength(40)]
        public string ContactPhone { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContactEmail { get; set; }

        [Range(1, 5000)]
        public int NumberOfGuests { get; set; }

        [MaxLength(2000)]
        public string Remarks { get; set; }

        [Range(1, 20)]
        public int BirthdayNo { get; set; }
        public decimal Price { get; set; }
        public int? OrderStatusId { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime StartDateAndTime { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime EndDateAndTime { get; set; }
    }
}