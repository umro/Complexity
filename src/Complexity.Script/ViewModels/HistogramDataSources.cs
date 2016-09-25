namespace Complexity.ViewModels
{
    public static class HistogramDataSources
    {
        public const string Fields = "Fields";
        public const string Plans  = "Plans";

        private static string[] _all = new string[]
        {
            Fields, Plans
        };

        public static string[] All
        {
            get { return _all; }
        }
    }
}
