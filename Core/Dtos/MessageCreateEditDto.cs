namespace Core.Dtos
{
    public class MessageCreateEditDto
    {
        public int Id { get; set; }
        public string FirstLastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MessageContent { get; set; }
        public bool IsReplied { get; set; }
    }
}