using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum AdType
    {
        Rent=1,
        Sale=2
    }

    public static class AdTypeExtensions
    {
        public static string ToArabic(this AdType adType) => adType switch
        {
            AdType.Rent => "ايجار",
            AdType.Sale => "بيع",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
