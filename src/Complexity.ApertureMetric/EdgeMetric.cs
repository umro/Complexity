namespace Complexity.ApertureMetric
{
    public class EdgeMetric
    {
        public double Calculate(Aperture aperture)
        {
            return DivisionOrDefault(aperture.SidePerimeter(),
                                     aperture.Area());
        }

        public static double DivisionOrDefault(double a, double b)
        {
            return (b != 0.0) ? (a / b) : 0.0;
        }
    }
}
