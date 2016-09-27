using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Complexity.Helpers;
using VMS.TPS.Common.Model.API;

namespace Complexity.ViewModels
{
    public class PlanViewModel : ViewModelBase
    {
        private double _penalty;
        private bool _showOnPlots;

        public Patient Patient { get; private set; }
        public PlanSetup Plan { get; private set; }

        public string Id
        {
            get { return Plan.Id; }
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

        public IEnumerable<FieldViewModel> Fields { get; set; }
        public ObservableCollection<FieldViewModel> SelectedFields { get; set; }

        public double MU
        {
            get { return (from field in Fields select field.MU).Sum(); }
        }

        public bool ShowOnPlots
        {
            get { return _showOnPlots; }
            set
            {
                _showOnPlots = value;
                NotifyPropertyChanged("ShowOnPlots");
            }
        }

        public PlanViewModel(Patient patient, PlanSetup plan)
        {
            Patient = patient;
            Plan = plan;

            OxyPlotColorsToUse Colors = new OxyPlotColorsToUse();
            Fields = (from Beam beam in plan.Beams
                      where !beam.IsSetupField
                      select new FieldViewModel(this, beam)
                      {
                          PlotColor = Colors.NextColor()
                      }).ToArray();

            // Add the first field to the list of selected fields
            SelectedFields = new ObservableCollection<FieldViewModel>();
            SelectedFields.Add(Fields.FirstOrDefault());
        }
    }
}
