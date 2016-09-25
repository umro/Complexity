namespace Complexity.ApertureMetric
{
    // Rectangular dimension (used for leaf and jaw positions);
    // it is relative to the top of the first leaf and the isocenter

    public class Rect
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }

        public Rect(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}
