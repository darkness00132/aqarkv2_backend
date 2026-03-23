namespace Domain.Enums
{
    public enum AdState
    {
        Unfinished,     // على الهيكل
        HalfFinished,   // نصف تشطيب
        FullyFinished,  // تشطيب كامل
        Furnished,      // مفروش
    }

    public static class AdStateExtensions
    {
        public static string ToArabic(this AdState adState) => adState switch
        {
            AdState.Unfinished => "على الهيكل",
            AdState.HalfFinished => "نصف تشطيب",
            AdState.FullyFinished => "تشطيب كامل",
            AdState.Furnished => "مفروش",
            _ => throw new ArgumentOutOfRangeException(nameof(adState), adState, null)
        };
    }
}