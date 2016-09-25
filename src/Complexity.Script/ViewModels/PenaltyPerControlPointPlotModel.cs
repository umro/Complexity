using System.Collections.Generic;
using System.Linq;
using Complexity.Helpers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Complexity.ViewModels
{
    public class PenaltyPerControlPointPlotModel : PlotModel
    {
        private Axis XAxis { get; set; }
        private Axis YAxis { get; set; }

        private OxyPlotColorsToUse ColorsToUse { get; set; }

        public PenaltyPerControlPointPlotModel(IList<FieldViewModel> fields, string yAxisTitle)
        {
            XAxis = CreateXAxis();
            YAxis = CreateYAxis(yAxisTitle);

            ColorsToUse = new OxyPlotColorsToUse();

            Axes.Add(CreateXAxis());
            Axes.Add(CreateYAxis(yAxisTitle));

            foreach (FieldViewModel field in fields)
            {
                LineSeries PenaltySeries = CreatePenaltySeries(field);
                Series.Add(PenaltySeries);
            }

            IsLegendVisible = false;
        }

        private Axis CreateXAxis()
        {
            return new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -180.0,
                Maximum =  180.0,
                StartPosition = 0.025,
                EndPosition = 0.975,
                MajorStep = 45.0,
                LabelFormatter = d => (d >= 0.0 ? d : d + 360.0).ToString(),
                Title = "Gantry angle",
                Unit = "degrees",
                TitleFontWeight = OxyPlot.FontWeights.Bold,
                FontSize = 18
            };
        }

        private Axis CreateYAxis(string yAxisTitle)
        {
            return new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0.0,
                Title = yAxisTitle,
                TitleFontWeight = OxyPlot.FontWeights.Bold,
                FontSize = 18
            };
        }

        private LineSeries CreatePenaltySeries(FieldViewModel field)
        {
            LineSeries series = new LineSeries();
            series.Title = field.PlotDisplayName;
            series.Points.AddRange(GetDataPoints(field).OrderBy(s => s.X));
            series.Color = ColorsToUse.NextColor();
            series.TrackerFormatString = "{0}\n{1}: {2:f6}\n{3}: {4:f6}";
            series.CanTrackerInterpolatePoints = false;
            field.PlotColor = series.Color;
            return series;
        }

        private IEnumerable<DataPoint> GetDataPoints(FieldViewModel field)
        {
            return from cp in field.ControlPoints
                   select new DataPoint(ShiftAngle(cp.GantryAngle), cp.Penalty);
        }

        // Convert 0 -> 360 degrees to -180 -> 180 degrees
        private double ShiftAngle(double angle)
        {
            return (angle < 180.0) ? angle : angle - 360.0;
        }
    }
}
