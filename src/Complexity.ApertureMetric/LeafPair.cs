using System;

namespace Complexity.ApertureMetric
{
    public class LeafPair
    {
        public Rect Position { get; private set; }

        public double Left   { get { return Position.Left;   } }
        public double Top    { get { return Position.Top;    } }
        public double Right  { get { return Position.Right;  } }
        public double Bottom { get { return Position.Bottom; } }

        public double Width { get; private set; }

        // Each leaf pair contains a reference to the jaw
        public Jaw Jaw { get; private set; }

        // Left and right represent the bank A and B, respectively
        public LeafPair(double left, double right,
            double width, double top, Jaw jaw)
        {
            Position = new Rect(left, top, right, top - width);
            Width = width;
            Jaw = jaw;
        }

        // Used to warn the user that there is a leaf behind the jaws,
        // even though it is open and within the top and bottom jaw edges
        public bool IsOpenButBehindJaw()
        {
            return (FieldSize() > 0.0) && (Jaw.Left > Left || Jaw.Right < Right);
        }

        public double FieldArea()
        {
            return FieldSize() * OpenLeafWidth();
        }

        // Returns the amount of leaf length that is open,
        // considering the position of the jaw
        public double FieldSize()
        {
            if (IsOutsideJaw())
            {
                return 0.0;
            }

            double left  = Math.Max(Jaw.Left,  Left);
            double right = Math.Min(Jaw.Right, Right);

            return right - left;
        }

        // Returns the amount of leaf width that is open,
        // considering the position of the jaw
        public double OpenLeafWidth()
        {
            if (IsOutsideJaw())
            {
                return 0.0;
            }

            double top    = Math.Min(Jaw.Top,    Top);
            double bottom = Math.Max(Jaw.Bottom, Bottom);

            return top - bottom;
        }

        public bool IsOutsideJaw()
        {
            // The reason for <= or >= instead of just < or >
            // is that if the jaw edge is equal to the leaf edge,
            // it's as if the jaw edge was the leaf edge,
            // so it's safer to count the leaf as outside,
            // so that the edges are not counted twice (leaf and jaw edge)
            return (Jaw.Top  <= Bottom) || (Jaw.Bottom >= Top) ||
                   (Jaw.Left >= Right)  || (Jaw.Right  <= Left);
        }

        public bool IsOpen()
        {
            return FieldSize() > 0.0;
        }
    }
}
