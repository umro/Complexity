namespace Complexity.ViewModels
{
    public static class PenaltyWeightTypes
    {
        public const string Weighted   = "Weighted (normal)";
        public const string Unweighted = "Unweighted (metric only)";
        public const string WeightOnly = "Weight only";

        private static string[] _all = new string[]
        {
            Weighted, Unweighted, WeightOnly
        };

        public static string[] All
        {
            get { return _all; }
        }
    }
}
