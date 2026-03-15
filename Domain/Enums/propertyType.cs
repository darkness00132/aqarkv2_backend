
namespace Domain.Enums
{
    public enum PropertyType
    {
        Apartment = 1,
        Villa = 2,
        House = 3,
        Land = 4,
        CommercialShop = 5,
        CommercialOffice = 6,
        Warehouse = 7,
        Studio = 8
    }
    public static class PropertyTypeExtensions
    {
        public static string ToArabic(this PropertyType propertyType) => propertyType switch
        {
            PropertyType.Apartment => "شقة",
            PropertyType.Villa => "فيلا",
            PropertyType.House => "منزل",
            PropertyType.Land => "أرض",
            PropertyType.CommercialShop => "محل تجاري",
            PropertyType.CommercialOffice => "مكتب تجاري",
            PropertyType.Warehouse => "مخزن",
            PropertyType.Studio => "استوديو",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
