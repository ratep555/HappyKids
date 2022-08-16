using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string FirstLastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(2000)]
        public string MessageContent { get; set; }
        public bool IsReplied { get; set; }

        [DataType(DataType.Date)]
        public DateTime SendingDate { get; set; } = DateTime.UtcNow;

    }
}