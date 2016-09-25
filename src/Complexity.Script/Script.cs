using System;
using System.Collections.Generic;
using System.Windows;
using Complexity;
using Complexity.ViewModels;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {
        public void Execute(ScriptContext scriptContext, Window mainWindow)
        {
            Run(scriptContext.CurrentUser,
                scriptContext.Patient,
                scriptContext.Image,
                scriptContext.StructureSet,
                scriptContext.PlanSetup,
                scriptContext.PlansInScope,
                scriptContext.PlanSumsInScope,
                mainWindow);
        }

        public void Run(
            User user,
            Patient patient,
            Image image,
            StructureSet structureSet,
            PlanSetup planSetup,
            IEnumerable<PlanSetup> planSetupsInScope,
            IEnumerable<PlanSum> planSumsInScope,
            Window mainWindow)
        {
            if (planSetup == null)
            {
                string message = "No active plan, please load a plan";
                MessageBox.Show(message);
                return;
            }

            try
            {
                var mainViewModel = new MainViewModel(user, patient, planSetup, planSetupsInScope);
                MainView mainView = new MainView(mainViewModel);
                mainWindow.Content = mainView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            }
        }
    }
}