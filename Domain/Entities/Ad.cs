using Domain.Enums;
using Domain.Identity;

namespace Domain.Entities
{
    public class Ad
    {
        public Guid Id { get; set; }

        public required string Slug { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required int Rooms { get; set; }

        public required double Space { get; set; }

        public required int Price { get; set; }

        public required string PropertyAddress { get; set; }

        public required AdType AdType { get; set; }

        public required PropertyType PropertyType { get; set; }

        public required int GovernorateId { get; set; }

        public required int CityId { get; set; }

        public required Guid UserId { get; set; }

        public User User { get; set; }

        public required ICollection<Image> Images { get; set; } = new List<Image>();

        public DateTime CreatedAt { get; set; }
    }
}
