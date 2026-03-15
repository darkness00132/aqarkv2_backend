using Domain.Enums;

namespace Application.DTOs.Ad
{
    public class UpdateAdDTO
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public int? Rooms { get; set; }

        public double? Space { get; set; }

        public int? Price { get; set; }

        public string? PropertyAddress { get; set; }

        public AdType? AdType { get; set; }

        public PropertyType? PropertyType { get; set; }
    }
}
