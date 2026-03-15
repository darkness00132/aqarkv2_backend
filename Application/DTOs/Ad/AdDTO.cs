using Application.DTOs.Image;
using Application.DTOs.User;
using Domain.Entities;

namespace Application.DTOs.Ad
{
    public class AdDTO
    {
        public string Slug { get; set; } = string.Empty;

        public required string Title { get; set; }

        public required string Description { get; set; }

        public int? Rooms { get; set; }

        public required double Space { get; set; }

        public required int Price { get; set; }

        public required string PropertyAddress { get; set; }

        public required string AdType { get; set; }

        public required string PropertyType { get; set; }

        public required PublicUser User { get; set; }

        public required string GovernorateName { get; set; }

        public required string CityName { get; set; }

        public required List<ImageDTO> Images { get; set; }

        public required DateTime CreatedAt { get; set; }
    }
}
