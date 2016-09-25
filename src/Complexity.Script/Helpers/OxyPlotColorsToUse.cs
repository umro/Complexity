using System.Collections.Generic;
using OxyPlot;

namespace Complexity.Helpers
{
    public class OxyPlotColorsToUse
    {
        private OxyColor[] Colors { get; set; }
        private int colorIndex;

        public OxyPlotColorsToUse()
        {
            List<OxyColor> colors = new List<OxyColor>();
            colors.Add(OxyColors.Blue);
            colors.Add(OxyColors.Red);
            colors.Add(OxyColors.Green);
            colors.Add(OxyColors.Orange);
            colors.Add(OxyColors.Purple);
            colors.Add(OxyColors.Gold);
            colors.Add(OxyColors.LightBlue);
            colors.Add(OxyColors.Pink);
            colors.Add(OxyColors.YellowGreen);
            colors.Add(OxyColors.OrangeRed);
            Colors = colors.ToArray();

            colorIndex = 0;
        }

        public OxyColor NextColor()
        {
            // Restart color index if it reaches the end of the list
            if (colorIndex == Colors.Length)
            {
                colorIndex = 0;
            }

            return Colors[colorIndex++];
        }
    }
}
