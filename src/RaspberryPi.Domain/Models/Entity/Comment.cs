using RaspberryPi.Domain.Core;

namespace RaspberryPi.Domain.Models.Entity
{
    public class Comment : Core.EntityBase
    {
        public Guid PageId { get; init; } = Guid.NewGuid();
        public string Text { get; init; }
        public DateTime DateCreatedUTC { get; set; }

        public Guid AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }
    }
}
