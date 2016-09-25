using VMS.TPS.Common.Model.API;

namespace Complexity.ViewModels
{
    public class ControlPointViewModel : ViewModelBase
    {
        private double _penalty;

        public ControlPoint ControlPoint { get; set; }

        public double GantryAngle { get; set; }

        public double Penalty
        {
            get { return _penalty; }
            set
            {
                _penalty = value;
                NotifyPropertyChanged("Penalty");
            }
        }

        public ControlPointViewModel(ControlPoint cp)
        {
            ControlPoint = cp;
            GantryAngle = ControlPoint.GantryAngle;
        }
    }
}
