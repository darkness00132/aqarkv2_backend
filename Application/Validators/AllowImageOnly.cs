using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Validators
{
    public class AllowImageOnly : ValidationAttribute
    {

        private readonly string[] _extensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private readonly string[] _contentTypes = { "image/jpeg", "image/png", "image/webp" };
        public bool AllowMultiple { get; init; } = false;
        public AllowImageOnly()
        {
            ErrorMessage ??= "Only image files (JPG, PNG, WebP) are allowed.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (AllowMultiple)
            {
                if (value is not IEnumerable<IFormFile> files)
                    return ValidationResult.Success;


                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!_extensions.Contains(extension) || !_contentTypes.Contains(file.ContentType.ToLower()))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }

                return ValidationResult.Success;
            }
            else
            {
                if (value is not IFormFile file)
                    return ValidationResult.Success;

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension) || !_contentTypes.Contains(file.ContentType.ToLower()))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }
    }
}
