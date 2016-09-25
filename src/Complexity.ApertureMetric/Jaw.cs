namespace Complexity.ApertureMetric
{
    public class Jaw
    {
        public Rect Position { get; set; }

        public double Left   { get { return Position.Left;   } }
        public double Top    { get { return Position.Top;    } }
        public double Right  { get { return Position.Right;  } }
        public double Bottom { get { return Position.Bottom; } }
    }
}
