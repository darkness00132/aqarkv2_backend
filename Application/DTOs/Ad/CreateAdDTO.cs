using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Application.Validators;

namespace Application.DTOs.Ad
{
    public class CreateAdDTO
    {
        public required string Title { get; set; }

        public required string Description { get; set; }

        [RequiredIfPropertyHasRooms]
        public int? Rooms { get; set; }

        public required double Space { get; set; }

        public required int Price { get; set; }

        public required string PropertyAddress { get; set; }

        public required AdType AdType { get; set; }

        public required PropertyType PropertyType { get; set; }

        public required int GovernorateId { get; set; }

        public required int CityId { get; set; }

        [MinLength(1, ErrorMessage = "يجب ارفاق صورة واحدة على الاقل")]
        [MaxLength(5, ErrorMessage = "عدد الصور المرفقة لا يجب ان يتجاوز ال 5 صور")]
        [AllowImageOnly(AllowMultiple=true)]
        public required IFormFile[] Images { get; set; }
    }
}
