using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Complexity.Helpers;
using VMS.TPS.Common.Model.API;

namespace Complexity.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(User user, Patient patient,
            PlanSetup activePlan, IEnumerable<PlanSetup> plans)
        {
            User = user;
            Patient = patient;
            Plan = activePlan;
            Plans = plans;

            EdgePenaltyViewModel = new EdgePenaltyViewModel(this, patient, activePlan, Plans);
        }

        public User User { get; }
        public Patient Patient { get; }
        public PlanSetup Plan { get; }    // Active plan
        public IEnumerable<PlanSetup> Plans { get; }

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string HelpUri =>
            Path.Combine(AssemblyHelper.GetAssemblyDirectory(), "ComplexityScript.pdf");

        public EdgePenaltyViewModel EdgePenaltyViewModel { get; }

        public void CalculateEdgePenalty()
        {
            EdgePenaltyViewModel.CalculateEdgePenalty();
        }
    }
}
