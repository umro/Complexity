using System;
using System.Linq;
using Complexity.Helpers;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Complexity.ViewModels
{
    public class HistogramModel : PlotModel
    {
        // Structure for each reference line to be drawn on the histogram
        public struct ReferenceLine
        {
            public double Value { get; set; }
            public string Text { get; set; }

            public ReferenceLine(double value, string text) : this()
            {
                Value = value;
                Text = text;
            }
        }

        // Hold the histogram information (bounds and counts)
        public Histogram Histogram { get; set; }

        // Individual data for which the histogram will be drawn
        private double[] data = new double[0];
        public double[] Data
        {
            get { return data; }
            set
            {
                data = value;
                DataChanged();
            }
        }

        // Reference lines to be drawn as labeled vertical lines
        private ReferenceLine[] refLines = new ReferenceLine[0];
        public ReferenceLine[] ReferenceLines
        {
            get { return refLines; }
            set
            {
                refLines = value;
                ReferenceLinesChanged();
            }
        }

        private Axis XAxis { get; set; }
        private Axis YAxis { get; set; }

        private void DataChanged()
        {
            UpdateHistogram();
            UpdateHistogramSeries();
            UpdateAxes();
            UpdateAnnotations();
        }

        private void UpdateHistogram()
        {
            Histogram = new Histogram(Data);
        }

        private void UpdateHistogramSeries()
        {
            Series.Clear();
            Series.Add(CreateHistogramSeries());
        }

        private RectangleBarSeries CreateHistogramSeries()
        {
            RectangleBarSeries series = new RectangleBarSeries()
            {
                StrokeThickness = 2.0
            };

            foreach (Histogram.Bin bin in Histogram.Bins)
            {
                if (bin.Count > 0)    // Skip drawing a bar with a count of 0
                {
                    series.Items.Add(CreateBar(bin));
                }
            }

            return series;
        }

        private RectangleBarItem CreateBar(Histogram.Bin bin)
        {
            return new RectangleBarItem(bin.LowerBound, 0.0, bin.UpperBound, bin.Count)
            {
                Color = OxyColors.Green
            };
        }

        private void UpdateAxes()
        {
            XAxis = CreateXAxis();
            YAxis = CreateYAxis();

            Axes.Add(XAxis);
            Axes.Add(YAxis);
        }

        private Axis CreateXAxis()
        {
            return new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0.0,
                Maximum = CalculateMaximumX(),
                Title = "Edge metric",
                FontSize = 18,
                TitleFontWeight = OxyPlot.FontWeights.Bold
            };
        }

        private double CalculateMaximumX()
        {
            if (Data.Length == 0)
            {
                return 0.0;
            }

            double dataMax = Data.Max();
            double histMax = (from bin in Histogram.Bins select bin.UpperBound).Max();
            return Math.Max(histMax, dataMax * 1.1);
        }

        private Axis CreateYAxis()
        {
            return new LinearAxis()
            {
                Position = AxisPosition.Left,
                Maximum = GetMaxHistogramCount() * 1.33,
                Title = "Frequency",
                Unit = "counts",
                TitleFontWeight = OxyPlot.FontWeights.Bold,
                FontSize = 18
            };
        }

        private double GetMaxHistogramCount()
        {
            return (from bin in Histogram.Bins select bin.Count).Max();
        }

        private void ReferenceLinesChanged()
        {
            UpdateAnnotations();
        }

        private void UpdateAnnotations()
        {
            Annotations.Clear();

            foreach (ReferenceLine refLine in ReferenceLines)
            {
                Annotations.Add(CreateReferenceLine(refLine));
            }

            Annotations.Add(CreateStatsText());
        }

        private LineAnnotation CreateReferenceLine(ReferenceLine refLine)
        {
            return new LineAnnotation()
            {
                Type = LineAnnotationType.Vertical,
                X = refLine.Value,
                Text = refLine.Text,
                Color = OxyColors.Red,
                StrokeThickness = 2.0,
                FontSize = 18.0,
                FontWeight = OxyPlot.FontWeights.Bold
            };
        }

        private TextAnnotation CreateStatsText()
        {
            return new TextAnnotation()
            {
                Text = BuildStatsText(),
                FontSize = 18.0,
                TextVerticalAlignment = VerticalAlignment.Top,
                TextHorizontalAlignment = HorizontalAlignment.Right,
                TextPosition = GetTopRightCorner(),
                StrokeThickness = 0.0,
            };
        }

        private string BuildStatsText()
        {
            return BuildSummaryStatsText();
        }

        private string BuildSummaryStatsText()
        {
            if (Data.Length == 0)
            {
                return "No data";
            }

            // There is an extra space at the end of each line of text
            // so that the text is not so close to the edge of the plot
            return string.Format("Count: {0} \nMean: {1:f4} \n",
                                 Data.Length, Data.Average());
        }

        private DataPoint GetTopRightCorner()
        {
            return new DataPoint(XAxis.Maximum, YAxis.Maximum);
        }
    }
}
