namespace RaspberryPi.Infrastructure.Models.Emails
{
    public class SendResponse
    {
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        public string Status { get; set; }
        public string CustomID { get; set; }
        public List<Recipient> To { get; set; }
        public List<object> Cc { get; set; }
        public List<object> Bcc { get; set; }
    }

    public class Recipient
    {
        public string Email { get; set; }
        public string MessageUUID { get; set; }
        public long MessageID { get; set; }
        public string MessageHref { get; set; }
    }
}
