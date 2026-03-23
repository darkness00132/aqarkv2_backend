using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Application.Validators;

namespace Application.DTOs.Ad
{
    public class CreateAdDTO
    {

        public required string Description { get; set; }

        [RequiredIfPropertyHasRooms]
        public int? Rooms { get; set; }

        [RequiredIfPropertyHasRooms]
        public int? Bathrooms { get; set; }

        public required double Space { get; set; }

        public required int Price { get; set; }

        public required string PropertyAddress { get; set; }

        public required AdType Type { get; set; }

        [RequiredIfPropertyHasRooms]
        public AdState? State { get; set; }

        public required PropertyType PropertyType { get; set; }

        public required int GovernorateId { get; set; }

        public required int CityId { get; set; }

        [AllowImageOnly(MinCount =1,MaxCount =5)]
        public required IFormFile[] Images { get; set; }
    }
}
