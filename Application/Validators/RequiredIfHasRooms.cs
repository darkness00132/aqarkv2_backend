using Application.DTOs.Ad;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Validators
{
    public class RequiredIfPropertyHasRoomsAttribute : ValidationAttribute
    {
        private readonly HashSet<PropertyType> _withRooms = new()
        {
            PropertyType.Apartment,
            PropertyType.Villa,
            PropertyType.House,
            PropertyType.Studio,
        };

        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            var dto = ctx.ObjectInstance as CreateAdDTO;
            if (dto == null) return ValidationResult.Success;

            if (_withRooms.Contains(dto.PropertyType) && (value == null || (int)value == 0))
                return new ValidationResult("عدد الغرف مطلوب لهذا النوع من العقارات");

            return ValidationResult.Success;
        }
    }
}
