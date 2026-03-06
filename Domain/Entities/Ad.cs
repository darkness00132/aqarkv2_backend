namespace Domain.Entities
{
    public class Ad
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required int Price { get; set; }

        public required Guid UserId { get; set; }

        public required ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
