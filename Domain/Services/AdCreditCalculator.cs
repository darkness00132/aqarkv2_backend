using Domain.Enums;

namespace Domain.Services
{
    public class AdCreditCalculator
    {
        private static readonly long[] SalePriceTierBoundaries =
        [
            500_000,
            1_000_000,
            2_000_000,
            5_000_000,
            10_000_000,
        ];

        private static readonly long[] RentPriceTierBoundaries =
        [
            2_000,
            5_000,
            10_000,
            25_000,
            50_000,
        ];

        private static readonly int[] PriceTierBonus = [0, 14, 28, 48, 72, 100];

        private const int FreeUpdateThreshold = 1_000;


        public static int CalculatePostCost(AdType adType, PropertyType propertyType, int price)
        {
            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");

            return GetBaseCredits(adType, propertyType) + PriceTierBonus[GetPriceTier(price, adType)];
        }

        public static int CalculateUpdateCost(int oldPrice, int newPrice, AdType adType, DateTimeOffset createdAt)
        {
            if (oldPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(oldPrice), "Old price must be positive.");
            if (newPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "New price must be positive.");

            // price drop or negligible change — always free
            if (newPrice <= oldPrice) return 0;
            if (newPrice - oldPrice < FreeUpdateThreshold) return 0;

            int oldBonus = PriceTierBonus[GetPriceTier(oldPrice, adType)];
            int newBonus = PriceTierBonus[GetPriceTier(newPrice, adType)];

            // same or lower tier — free
            if (newBonus <= oldBonus) return 0;

            int tierDifference = newBonus - oldBonus;
            double decayFactor = GetAgeDecayFactor(createdAt);

            int cost = (int)Math.Ceiling(tierDifference * decayFactor);

            return Math.Max(1, cost);
        }


        private static double GetAgeDecayFactor(DateTimeOffset createdAt)
        {
            int ageDays = (int)(DateTimeOffset.UtcNow - createdAt).TotalDays;

            return ageDays switch
            {
                <= 7 => 1.00, // week 1 — full cost
                <= 14 => 0.75, // week 2 — 75%
                <= 21 => 0.50, // week 3 — half
                _ => 0.25, // week 4+ — quarter
            };
        }

        private static int GetPriceTier(long price, AdType adType)
        {
            long[] boundaries = adType.IsRent()
                ? RentPriceTierBoundaries
                : SalePriceTierBoundaries;

            for (int i = 0; i < boundaries.Length; i++)
            {
                if (price < boundaries[i])
                    return i;
            }
            return boundaries.Length;
        }

        private static int GetBaseCredits(AdType adType, PropertyType propertyType)
        {
            return propertyType switch
            {
                PropertyType.Apartment or
                PropertyType.Studio => adType switch
                {
                    AdType.RentMonthly => 85,
                    AdType.RentYearly => 83,
                    AdType.RentSeasonal => 95,
                    AdType.SaleCash => 106,
                    AdType.SaleInstallment => 114,
                    _ => 85
                },

                PropertyType.House or
                PropertyType.Chalet => adType switch
                {
                    AdType.RentMonthly => 89,
                    AdType.RentYearly => 87,
                    AdType.RentSeasonal => 99,
                    AdType.SaleCash => 111,
                    AdType.SaleInstallment => 119,
                    _ => 89
                },

                PropertyType.Villa => adType switch
                {
                    AdType.RentMonthly => 100,
                    AdType.RentYearly => 98,
                    AdType.RentSeasonal => 111,
                    AdType.SaleCash => 132,
                    AdType.SaleInstallment => 141,
                    _ => 100
                },

                PropertyType.Compound => adType switch
                {
                    AdType.RentMonthly => 104,
                    AdType.RentYearly => 102,
                    AdType.RentSeasonal => 115,
                    AdType.SaleCash => 137,
                    AdType.SaleInstallment => 146,
                    _ => 104
                },

                PropertyType.CommercialShop or
                PropertyType.Office => adType switch
                {
                    AdType.RentMonthly => 109,
                    AdType.RentYearly => 106,
                    AdType.RentSeasonal => 119,
                    AdType.SaleCash => 142,
                    AdType.SaleInstallment => 151,
                    _ => 109
                },

                PropertyType.MedicalClinic => adType switch
                {
                    AdType.RentMonthly => 121,
                    AdType.RentYearly => 118,
                    AdType.RentSeasonal => 132,
                    AdType.SaleCash => 158,
                    AdType.SaleInstallment => 168,
                    _ => 121
                },

                PropertyType.Hotel => adType switch
                {
                    AdType.RentMonthly => 132,
                    AdType.RentYearly => 129,
                    AdType.RentSeasonal => 145,
                    AdType.SaleCash => 176,
                    AdType.SaleInstallment => 187,
                    _ => 132
                },

                PropertyType.Building => adType switch
                {
                    AdType.RentMonthly => 124,
                    AdType.RentYearly => 121,
                    AdType.RentSeasonal => 135,
                    AdType.SaleCash => 166,
                    AdType.SaleInstallment => 176,
                    _ => 124
                },

                PropertyType.Farm => adType switch
                {
                    AdType.RentMonthly => 95,
                    AdType.RentYearly => 93,
                    AdType.RentSeasonal => 106,
                    AdType.SaleCash => 126,
                    AdType.SaleInstallment => 135,
                    _ => 95
                },

                PropertyType.Land => adType switch
                {
                    AdType.RentMonthly => 91,
                    AdType.RentYearly => 89,
                    AdType.RentSeasonal => 101,
                    AdType.SaleCash => 145,
                    AdType.SaleInstallment => 155,
                    _ => 91
                },

                PropertyType.Warehouse => adType switch
                {
                    AdType.RentMonthly => 93,
                    AdType.RentYearly => 91,
                    AdType.RentSeasonal => 103,
                    AdType.SaleCash => 122,
                    AdType.SaleInstallment => 131,
                    _ => 93
                },

                PropertyType.Garage => adType switch
                {
                    AdType.RentMonthly => 85,
                    AdType.RentYearly => 83,
                    AdType.RentSeasonal => 93,
                    AdType.SaleCash => 103,
                    AdType.SaleInstallment => 110,
                    _ => 85
                },

                _ => adType.IsSale() ? 106 : 85
            };
        }
    }
}