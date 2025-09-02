namespace Chat_app.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Message> Messages { get; set; } = new();
    }

    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public int ChatGroupId { get; set; }
        public ChatGroup? ChatGroup { get; set; }
    }
}
