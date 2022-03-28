using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.BirthdayOrdersDtos
{
    public class ClientBirthdayOrderCreateDto
    {
        public int BranchId { get; set; }
        public int BirthdayPackageId { get; set; }

        [Required]
        [MaxLength(70)]
        public string ClientName { get; set; }

        [Required]
        [MaxLength(30)]
        public string BirthdayGirlBoyName { get; set; }

        [Required]
        [MaxLength(30)]
        public string ContactPhone { get; set; }

        [Required]
        [MaxLength(30)]
        public string ContactEmail { get; set; }

        [Required]
        [Range(1, 500)]
        public int NumberOfGuests { get; set; }

        [MaxLength(2000)]
        public string Remarks { get; set; }

        [Required]
        [Range(1, 20)]
        public int BirthdayNo { get; set; }
        public int? OrderStatusId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDateAndTime { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDateAndTime { get; set; }
    }
}