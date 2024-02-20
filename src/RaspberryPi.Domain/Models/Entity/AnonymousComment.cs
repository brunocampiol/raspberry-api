namespace RaspberryPi.Domain.Models.Entity
{
    public class AnonymousComment : BaseEntity
    {
        public string Text { get; init; } = default!;
        public DateTime DateCreatedUTC { get; set; }
    }
}