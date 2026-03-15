using Application.DTOs.Image;
using Application.DTOs.User;

namespace Application.DTOs.Ad
{
    public class AdListItemDTO
    {
        public required string Slug { get; set; }

        public required string Title { get; set; }
        public int? Rooms { get; set; }
        public required double Space { get; set; }
        public required int Price { get; set; }
        public required string AdType { get; set; }
        public required string PropertyType { get; set; }
        public required string GovernorateName { get; set; }
        public required string CityName { get; set; }
        public required PublicUser User { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required ImageDTO CoverImage { get; set; }
    }
}
