using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OxyPlot;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Complexity.ViewModels
{
    public class FieldViewModel : ViewModelBase
    {
        private double _penalty;

        public PlanViewModel Plan { get; set; }
        public Beam Beam { get; set; }

        public string DisplayName { get; set; }

        public string PlotDisplayName { get; set; }

        private OxyColor _plotColor;

        public OxyColor PlotColor
        {
            get
            {
                if (Plan.ShowOnPlots && ShowOnPlots)
                {
                    return _plotColor;
                }
                else
                {
                    return OxyColors.Transparent;
                }
            }
            set
            {
                if (!_plotColor.Equals(value))
                {
                    _plotColor = value;
                    NotifyPropertyChanged("PlotColor");
                }
            }
        }

        public double Penalty
        {
            get { return _penalty; }
            set
            {
                _penalty = value;
                NotifyPropertyChanged("Penalty");
            }
        }

        public double MU
        {
            get
            {
                MetersetValue mv = Beam.Meterset;
                if (mv.Unit == DosimeterUnit.MU)
                {
                    return mv.Value;
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        private bool _showOnPlots;
        public bool ShowOnPlots
        {
            get { return _showOnPlots; }
            set
            {
                _showOnPlots = value;
                NotifyPropertyChanged("ShowOnPlots");
                NotifyPropertyChanged("PlotColor");
            }
        }

        public IEnumerable<ControlPointViewModel> ControlPoints { get; set; }

        public FieldViewModel(PlanViewModel plan, Beam beam)
        {
            Plan = plan;
            Beam = beam;
            DisplayName = Beam.Id;
            PlotDisplayName = Plan.Plan.Id + ": " + Beam.Id;
            PlotColor = OxyColors.Black;
            ControlPoints = (from ControlPoint cp in beam.ControlPoints
                             select new ControlPointViewModel(cp)).ToArray();

            // Show field by default
            // (won't actually be shown until plan's ShowOnPlots is true)
            ShowOnPlots = true;

            Plan.PropertyChanged += PlanPropertyChanged;
        }

        private void PlanPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowOnPlots")
            {
                NotifyPropertyChanged("PlotColor");
            }
        }
    }
}
