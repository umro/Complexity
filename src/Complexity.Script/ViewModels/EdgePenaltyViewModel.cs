using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using Complexity.EsapiApertureMetric;
using Complexity.Helpers;
using OxyPlot;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Complexity.ViewModels
{
    public class EdgePenaltyViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; }

        private EdgeMetric EdgePenaltyCalculator { get; set; }

        private string VmatDataPath
        {
            get { return GetPathForFileName("vmat.csv"); }
        }

        private string ImrtDataPath
        {
            get { return GetPathForFileName("imrt.csv"); }
        }

        private string GetPathForFileName(string fileName)
        {
            return Path.Combine(AssemblyHelper.GetAssemblyDirectory(), fileName);
        }

        private PatientData Data { get; set; }

        public ObservableCollection<CourseViewModel> Courses { get; set; }

        public ObservableCollection<PlanViewModel> Plans { get; set; }

        public ObservableCollection<FieldViewModel> Fields { get; set; }

        #region Observable properties

        private bool _isBoxPlot;
        public bool IsBoxPlot
        {
            get { return _isBoxPlot; }
            set
            {
                _isBoxPlot = value;
                NotifyPropertyChanged("IsBoxPlot");
            }
        }

        private PlotModel _plotModel;
        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                NotifyPropertyChanged("PlotModel");
            }
        }

        private string _weightType;
        public string WeightType
        {
            get { return _weightType; }
            set
            {
                _weightType = value;
                NotifyPropertyChanged("WeightType");
            }
        }

        private bool _showHistogramLastUpdated;
        public bool ShowHistogramLastUpdated
        {
            get { return _showHistogramLastUpdated; }
            set
            {
                _showHistogramLastUpdated = value;
                NotifyPropertyChanged("ShowHistogramLastUpdated");
            }
        }

        private DateTime _histogramLastUpdatedOn;
        public DateTime HistogramLastUpdatedOn
        {
            get { return _histogramLastUpdatedOn; }
            set
            {
                _histogramLastUpdatedOn = value;
                NotifyPropertyChanged("HistogramLastUpdatedOn");
            }
        }

        private string _histogramLastUpdatedBy;
        public string HistogramLastUpdatedBy
        {
            get { return _histogramLastUpdatedBy; }
            set
            {
                _histogramLastUpdatedBy = value;
                NotifyPropertyChanged("HistogramLastUpdatedBy");
            }
        }

        private bool _isVmat;
        public bool IsVmat
        {
            get { return _isVmat; }
            set
            {
                _isVmat = value;
                NotifyPropertyChanged("IsVmat");
            }
        }

        #endregion // Observable properties

        public string[] WeightTypes
        {
            get { return PenaltyWeightTypes.All; }
        }

        public HistogramViewModel HistogramViewModel { get; set; }

        public EdgePenaltyViewModel(MainViewModel mainViewModel,
            Patient patient, PlanSetup activePlan, IEnumerable<PlanSetup> plans)
        {
            MainViewModel = mainViewModel;

            UserMessaged += UserMessager.UserMessaged;

            Plans = new ObservableCollection<PlanViewModel>
                (from plan in plans
                    select new PlanViewModel(patient, plan));

            foreach (PlanViewModel plan in Plans)
            {
                plan.PropertyChanged += OnPlanPropertyChanged;

                foreach (FieldViewModel field in plan.Fields)
                {
                    field.PropertyChanged += OnFieldPropertyChanged;
                }
            }

            // Select active plan and its fields for showing on plot
            PlanViewModel activePlanViewModel = Plans.FirstOrDefault(p => p.Plan == activePlan);
            if (activePlanViewModel != null)
            {
                activePlanViewModel.ShowOnPlots = true;
                foreach (FieldViewModel field in activePlanViewModel.Fields)
                {
                    field.ShowOnPlots = true;
                }
            }

            // Get all fields for all plans
            Fields = new ObservableCollection<FieldViewModel>
                (from plan in Plans
                 from field in plan.Fields
                 select field);

            Courses = new ObservableCollection<CourseViewModel>
                (from course in GetCourses(plans)
                 let fields = Fields.Where(f => f.Plan.Plan.Course == course)
                 select new CourseViewModel(course, fields));

            EdgePenaltyCalculator = new EdgeMetric();

            WeightType = WeightTypes[0];

            HistogramViewModel = new HistogramViewModel(Plans);

            // Choose plot type (box or line) and histogram therapy type (VMAT or IMRT)
            // based on the type of the active plan
            var firstBeam = activePlan.Beams.FirstOrDefault();
            IsVmat = firstBeam != null && firstBeam.MLCPlanType == MLCPlanType.VMAT;
            OnIsVmatChanged();
            IsBoxPlot = !IsVmat;   // Box for IMRT, Line for VMAT
            OnIsBoxPlotChanged();

            PropertyChanged += OnPropertyChanged;
        }

        private IEnumerable<Course> GetCourses(IEnumerable<PlanSetup> plans)
        {
            if (plans == null || !plans.Any())
            {
                return Enumerable.Empty<Course>();
            }

            return (from plan in plans
                    orderby plan.Course.Id
                    select plan.Course).Distinct();
        }

        private void OnPlanPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ShowOnPlots":
                    OnPlanShowOnPlotsChanged();
                    break;
            }
        }

        private void OnFieldPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePlotModel();
        }

        #region PropertyChanged handlers

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "WeightType":
                    OnWeightTypePropertyChanged();
                    break;
                case "IsVmat":
                    OnIsVmatChanged();
                    break;
                case "IsBoxPlot":
                    OnIsBoxPlotChanged();
                    break;
            }
        }

        private void OnWeightTypePropertyChanged()
        {
            switch (WeightType)
            {
                case PenaltyWeightTypes.Weighted:
                    CalculateEdgePenalty();
                    break;

                case PenaltyWeightTypes.Unweighted:
                    CalculateUnweightedEdgePenaltiesForControlPointsOnly();
                    break;

                case PenaltyWeightTypes.WeightOnly:
                    CalculateWeightsOnlyForControlPointsOnly();
                    break;

                default:
                    throw new ApplicationException("Weight type not supported");
            }

            UpdatePlotModel();
        }

        private void OnIsVmatChanged()
        {
            HistogramViewModel.Data = IsVmat ? new PatientData(VmatDataPath)
                                             : new PatientData(ImrtDataPath);
        }

        private void OnPlanShowOnPlotsChanged()
        {
            UpdatePlotModel();
        }

        private void OnIsBoxPlotChanged()
        {
            UpdatePlotModel();
        }

        #endregion // PropertyChanged handlers

        private void UpdatePlotModel()
        {
            PlotModel = CreatePlotModel();
        }

        public void CalculateEdgePenalty()
        {
            if (Plans != null)
            {
                foreach (PlanViewModel plan in Plans)
                {
                    try
                    {
                        plan.Penalty = EdgePenaltyCalculator
                            .CalculateForPlan(plan.Patient, plan.Plan);

                        foreach (FieldViewModel field in plan.Fields)
                        {
                            CalculateEdgePenaltyForField(field);
                        }
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;

                        if (e.InnerException != null)
                        {
                            message += "\n" + e.InnerException.Message;
                        }

                        NotifyUserMessaged(message, UserMessageType.Error);
                    }
                }
            }

            UpdatePlotModel();
            HistogramViewModel.Refresh();
        }

        public void CalculateEdgePenaltyForField(FieldViewModel field)
        {
            double[] edgePenalties = EdgePenaltyCalculator.
                CalculatePerControlPointWeighted(field.Plan.Patient, field.Plan.Plan, field.Beam);

            field.Penalty = edgePenalties.Sum();

            SetEdgePenaltiesForControlPoints(field, edgePenalties);
        }

        public void CalculateUnweightedEdgePenaltiesForControlPointsOnly()
        {
            if (Plans != null)
            {
                foreach (PlanViewModel plan in Plans)
                {
                    try
                    {
                        foreach (FieldViewModel field in plan.Fields)
                        {
                            double[] penalties = EdgePenaltyCalculator.
                                CalculatePerControlPointUnweighted(
                                    field.Plan.Patient, field.Plan.Plan, field.Beam);
                            SetEdgePenaltiesForControlPoints(field, penalties);
                        }
                    }
                    catch (Exception e)
                    {
                        NotifyUserMessaged(e.Message, UserMessageType.Error);
                    }
                }
            }
        }

        public void CalculateWeightsOnlyForControlPointsOnly()
        {
            if (Plans != null)
            {
                foreach (PlanViewModel plan in Plans)
                {
                    foreach (FieldViewModel field in plan.Fields)
                    {
                        double[] penalties = EdgePenaltyCalculator.
                            CalculatePerControlPointWeightsOnly(field.Beam);
                        SetEdgePenaltiesForControlPoints(field, penalties);
                    }
                }
            }
        }

        private void SetEdgePenaltiesForControlPoints(FieldViewModel field, double[] penalties)
        {
            int i = 0;
            foreach (ControlPointViewModel cp in field.ControlPoints)
            {
                cp.Penalty = penalties[i++];
            }
        }

        public PlotModel CreatePlotModel()
        {
            // Get fields to show across all plans
            var fields = from plan in Plans
                         where plan.ShowOnPlots
                         from field in plan.Fields
                         where field.ShowOnPlots
                         select field;

            if (IsBoxPlot)
            {
                return new BoxPlotPerFieldPlotModel(fields.ToList(), GetYAxisTitle());
            }
            else
            {
                return new PenaltyPerControlPointPlotModel(fields.ToList(), GetYAxisTitle());
            }
        }

        public string GetYAxisTitle()
        {
            if (WeightType == PenaltyWeightTypes.Weighted)
            {
                return "Edge metric";
            }
            else if (WeightType == PenaltyWeightTypes.Unweighted)
            {
                return "Unweighted edge metric";
            }
            else if (WeightType == PenaltyWeightTypes.WeightOnly)
            {
                return "Weight only (MU)";
            }
            else
            {
                return "Invalid";
            }
        }
    }
}
