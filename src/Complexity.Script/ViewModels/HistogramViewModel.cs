using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Complexity.ViewModels
{
    public class HistogramViewModel : ViewModelBase
    {
        private IEnumerable<PlanViewModel> Plans { get; set; }

        private PatientData _data;
        public PatientData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                NotifyPropertyChanged("Data");
            }
        }

        private HistogramModel _histogramModel;
        public HistogramModel HistogramModel
        {
            get { return _histogramModel; }
            set
            {
                _histogramModel = value;
                NotifyPropertyChanged("HistogramModel");
            }
        }

        // Data to show (plan or field)
        private string _dataType;
        public string DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value;
                NotifyPropertyChanged("DataType");
            }
        }

        public string[] DataTypes
        {
            get { return new string[] { "Plans", "Fields" }; }
        }

        private string _site;
        public string Site
        {
            get { return _site; }
            set
            {
                _site = value;
                NotifyPropertyChanged("Site");
            }
        }

        public string[] Sites
        {
            get { return GetSites(); }
        }

        private string[] GetSites()
        {
            List<string> sites = new List<string>();

            if (Data != null)
            {
                sites.AddRange(Data.GetUniqueSites());
            }

            sites.Insert(0, "All sites");
            return sites.ToArray();
        }

        public ObservableCollection<StatViewModel> Stats { get; set; }

        public HistogramViewModel(IEnumerable<PlanViewModel> plans)
        {
            Plans = plans;

            foreach (PlanViewModel plan in Plans)
            {
                plan.SelectedFields.CollectionChanged += OnSelectedFieldsChanged;
                plan.PropertyChanged += OnPlanPropertyChanged;

                foreach (FieldViewModel field in plan.Fields)
                {
                    field.PropertyChanged += OnFieldPropertyChanged;
                }
            }

            Stats = new ObservableCollection<StatViewModel>();

            PropertyChanged += OnPropertyChanged;
        }

        private void OnSelectedFieldsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        private void OnPlanPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }

        private void OnFieldPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Prevent further property changes to call this method
            PropertyChanged -= OnPropertyChanged;

            switch (e.PropertyName)
            {
                case "Data":
                    OnDataChanged();
                    break;
            }

            // Refresh histogram when any property changes
            Refresh();

            PropertyChanged += OnPropertyChanged;
        }

        private void OnDataChanged()
        {
            // These properties may change when the data changes
            NotifyPropertyChanged("DataType");
            NotifyPropertyChanged("Sites");

            if (DataType == null)
            {
                DataType = DataTypes.First();
            }

            if (Site == null)
            {
                Site = Sites.First();
            }
        }

        public void Refresh()
        {
            HistogramModel = CreateHistogramModel();
            UpdateStats();
        }

        private HistogramModel CreateHistogramModel()
        {
            if (DataType == "Fields")
            {
                return CreateFieldHistogramModel();
            }
            else if (DataType == "Plans")
            {
                return CreatePlanHistogramModel();
            }

            throw new ApplicationException
                (string.Format("Unknown data type \"{0}\". " +
                    "Only \"Fields\" or \"Plans\" are allowed.", DataType));
        }

        #region Field histogram model

        private HistogramModel CreateFieldHistogramModel()
        {
            return new HistogramModel()
            {
                Data = GetFieldData(),
                ReferenceLines = GetFieldReferenceLines()
            };
        }

        private double[] GetFieldData()
        {
            if (Site == "All sites")
            {
                return Data.GetFieldMetrics();
            }
            else
            {
                return Data.GetFieldMetrics(Site);
            }
        }

        private HistogramModel.ReferenceLine[] GetFieldReferenceLines()
        {
            return (from plan in Plans
                    where plan.ShowOnPlots
                    from field in plan.Fields
                    where field.ShowOnPlots
                    let fieldName = plan.Plan.Id + ": " + field.DisplayName
                    select new HistogramModel.ReferenceLine
                        (field.Penalty, fieldName)).ToArray();
        }

        #endregion // Field histogram model

        #region Plan histogram model

        private HistogramModel CreatePlanHistogramModel()
        {
            return new HistogramModel()
            {
                Data = GetPlanData(),
                ReferenceLines = GetPlanReferenceLines()
            };
        }

        private double[] GetPlanData()
        {
            if (Site == "All sites")
            {
                return Data.GetPlanMetrics();
            }
            else
            {
                return Data.GetPlanMetrics(Site);
            }
        }

        private HistogramModel.ReferenceLine[] GetPlanReferenceLines()
        {
            return (from plan in Plans
                    where plan.ShowOnPlots
                    let planName = plan.Plan.Id
                    select new HistogramModel.ReferenceLine(plan.Penalty, planName)).ToArray();
        }

        #endregion // Plan histogram model

        private void UpdateStats()
        {
            Stats.Clear();

            foreach (var refLine in HistogramModel.ReferenceLines)
            {
                try    // StdDevs may fail if there's no data
                {
                    Stats.Add(new StatViewModel
                    {
                        Name = refLine.Text,
                        StdDev = Helpers.Stats.StdDevs(refLine.Value, HistogramModel.Data)
                    });
                }
                catch (Exception)
                {
                    // Skip this plan or field
                }
            }
        }
    }
}
