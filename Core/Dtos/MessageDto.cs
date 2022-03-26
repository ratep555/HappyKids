using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string FirstLastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MessageContent { get; set; }
        public bool IsReplied { get; set; }

        [DataType(DataType.Date)]
        public DateTime SendingDate { get; set; }
    }
}


