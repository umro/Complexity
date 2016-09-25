using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Complexity.Helpers;
using Complexity.ViewModels;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;
using VMS.TPS.Common.Model.API;

namespace Complexity
{
    public partial class MainView : UserControl
    {
        private readonly MainViewModel _mainViewModel;

        // Public properties to get API data from Start
        public Patient Patient { get; set; }
        public Course Course { get; set; }
        public PlanSetup Plan { get; set; }     // Active plan
        public IEnumerable<PlanSetup> Plans { get; set; }
        public User User { get; set; }

        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            RegisterViewModelEventHandlers();

            DataContext = _mainViewModel;
        }

        private void MainView_OnLoaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            var waitWindow = new WaitWindow();
            waitWindow.Show();

            _mainViewModel.CalculateEdgePenalty();

            waitWindow.Close();

            Mouse.OverrideCursor = null;
        }

        private void RegisterViewModelEventHandlers()
        {
            // These event handlers invalidate the related plots
            // because they cannot be updated through binding
            _mainViewModel.EdgePenaltyViewModel.PropertyChanged +=
                OnEdgePenaltyViewModelPropertyChanged;
        }

        void OnEdgePenaltyViewModelPropertyChanged(object sender,
            PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PlotModel":
                    Plot.InvalidatePlot();
                    break;

                case "HistogramModel":
                    Histogram.InvalidatePlot();
                    break;
            }
        }

        private void ExportControlPointPlotAsPdf(object sender, RoutedEventArgs e)
        {
            ExportPlotAsPdf(Plot, _mainViewModel.EdgePenaltyViewModel.PlotModel);
        }

        private void ExportHistogramAsPdf(object sender, RoutedEventArgs e)
        {
            ExportPlotAsPdf(Histogram, _mainViewModel.EdgePenaltyViewModel.HistogramViewModel.HistogramModel);
        }

        private void ExportPlotAsPdf(PlotView plotView, PlotModel plotModel)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export to PDF",
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var stream = File.Create(saveFileDialog.FileName))
                    {
                        double width = 600.0;     // Fix the width, but determine the height
                        double height = width*(plotView.ActualHeight/plotView.ActualWidth);
                        PdfExporter.Export(plotModel, stream, width, height);
                    }
                }
                catch (Exception ex)
                {
                    _mainViewModel.NotifyUserMessaged("Unable to export to a PDF file. " + ex.Message, UserMessageType.Error);
                }
            }
        }

        private void ExportControlPointDataAsCsv(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export data to CSV",
                Filter = "CSV Files (*.csv)|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    List<string> output = new List<string>();

                    // Header
                    output.Add("Course,Plan,Field,Gantry Angle," + _mainViewModel.EdgePenaltyViewModel.GetYAxisTitle());

                    var plansToShow = _mainViewModel.EdgePenaltyViewModel.Plans.Where(p => p.ShowOnPlots);
                    foreach (var plan in plansToShow)
                    {
                        string courseId = plan.Plan.Course.Id;

                        var fieldsToShow = plan.Fields.Where(f => f.ShowOnPlots);
                        foreach (var field in fieldsToShow)
                        {
                            foreach (var cp in field.ControlPoints)
                            {
                                output.Add(string.Format("{0},{1},{2},{3},{4}",
                                    courseId, plan.Id, field.Beam.Id,
                                    ShiftAngle(cp.GantryAngle), cp.Penalty));
                            }
                        }
                    }

                    File.WriteAllLines(saveFileDialog.FileName, output);
                }
                catch (Exception ex)
                {
                    _mainViewModel.NotifyUserMessaged("Unable to export to a CSV file. " + ex.Message, UserMessageType.Error);
                }
            }
        }

        // Convert 0 -> 360 degrees to -180 -> 180 degrees
        private double ShiftAngle(double angle)
        {
            return (angle < 180.0) ? angle : angle - 360.0;
        }

        private void ExportHistogramAsCsv(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Export data to CSV",
                Filter = "CSV Files (*.csv)|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    List<string> output = new List<string>();

                    // Header
                    output.Add("LowerBound,UpperBound,Count");

                    foreach (
                        Histogram.Bin bin in
                            _mainViewModel.EdgePenaltyViewModel.HistogramViewModel.HistogramModel.Histogram.Bins)
                    {
                        output.Add(string.Format("{0},{1},{2}", bin.LowerBound, bin.UpperBound, bin.Count));
                    }

                    File.WriteAllLines(saveFileDialog.FileName, output);
                }
                catch (Exception ex)
                {
                    _mainViewModel.NotifyUserMessaged("Unable to export to a CSV file. " + ex.Message, UserMessageType.Error);
                }
            }
        }
    }
}