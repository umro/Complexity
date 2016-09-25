using System.Collections.Generic;
using System.Linq;
using Complexity.Helpers;
using MathNet.Numerics.Statistics;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Complexity.ViewModels
{
    public class BoxPlotPerFieldPlotModel : PlotModel
    {
        private OxyPlotColorsToUse ColorsToUse { get; set; }

        public BoxPlotPerFieldPlotModel(IList<FieldViewModel> fields, string yAxisTitle)
        {
            Axes.Add(CreateXAxis(fields));
            Axes.Add(CreateYAxis(yAxisTitle));

            ColorsToUse = new OxyPlotColorsToUse();

            int i = 0;
            foreach (FieldViewModel field in fields)
            {
                BoxPlotSeries series = CreateSeries(field, i++);
                Series.Add(series);
            }
        }

        private Axis CreateXAxis(IEnumerable<FieldViewModel> fields)
        {
            return new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Field name",
                ItemsSource = fields,
                LabelField = "DisplayName",
                TitleFontWeight = FontWeights.Bold,
                FontSize = 18
            };
        }

        private Axis CreateYAxis(string yAxisTitle)
        {
            return new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0.0,
                Title = yAxisTitle,
                TitleFontWeight = FontWeights.Bold,
                FontSize = 18
            };
        }

        private BoxPlotSeries CreateSeries(FieldViewModel field, int index)
        {
            // A single series may contain multiple boxes,
            // but the problem is that they must all share the same color,
            // so here each series contains a single box,
            // so that each box can have its own color
            BoxPlotSeries series = new BoxPlotSeries();
            series.Items = new[] {GetBox(field, index)};
            series.MeanThickness = 0.0;
            series.Stroke = ColorsToUse.NextColor();
            field.PlotColor = series.Stroke;
            return series;
        }

        private BoxPlotItem GetBox(FieldViewModel field, int index)
        {
            double[] data = field.ControlPoints
                                .OrderBy(cp => cp.Penalty)
                                .Select(cp => cp.Penalty)
                                .ToArray();

            var box = new BoxPlotItem
            {
                X = index,
                LowerWhisker = data.Quantile(0.02),
                BoxBottom = data.LowerQuartile(),
                Median = data.Median(),
                BoxTop = data.UpperQuartile(),
                UpperWhisker = data.Quantile(0.98)
            };
            box.Outliers = data.Where(d => d < box.LowerWhisker || d > box.UpperWhisker).ToList();

            return box;
        }
    }
}
