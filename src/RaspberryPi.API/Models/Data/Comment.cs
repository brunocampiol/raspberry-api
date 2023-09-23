namespace RaspberryPi.API.Models.Data
{
    public class Comment : BaseEntity
    {
        public Guid PageId { get; init; } = Guid.NewGuid();
        public string Text { get; init; }
        public DateTime DateCreatedUTC { get; set; }

        public Guid AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }
    }
}