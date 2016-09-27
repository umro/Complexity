using System;
using System.Collections.Generic;
using System.Linq;
using Complexity.ApertureMetric;
using Complexity.AriaEntity;
using VMS.TPS.Common.Model.API;
using Patient = VMS.TPS.Common.Model.API.Patient;
using PlanSetup = VMS.TPS.Common.Model.API.PlanSetup;

namespace Complexity.EsapiApertureMetric
{
    public class AperturesFromBeamCreator
    {
        public IEnumerable<Aperture> Create(Patient patient, PlanSetup plan, Beam beam)
        {
            List<Aperture> apertures = new List<Aperture>();
            double[] leafWidths = GetLeafWidths(patient, plan, beam);

            foreach (ControlPoint controlPoint in beam.ControlPoints)
            {
                double[,] leafPositions = GetLeafPositions(controlPoint);
                double[] jaw = CreateJaw(controlPoint);
                apertures.Add(new Aperture(leafPositions, leafWidths, jaw));
            }

            return apertures;
        }

        private double[] CreateJaw(ControlPoint cp)
        {
            double left   = cp.JawPositions.X1;
            double top    = cp.JawPositions.Y2;
            double right  = cp.JawPositions.X2;
            double bottom = cp.JawPositions.Y1;

            return new double[] { left, top, right, bottom };
        }

        public double[,] GetLeafPositions(ControlPoint controlPoint)
        {
            int m = controlPoint.LeafPositions.GetLength(0);
            int n = controlPoint.LeafPositions.GetLength(1);

            double[,] leafPositions = new double[m, n];

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    // Leaf positions are given from bottom to top by ESAPI,
                    // but the Aperture class expects them from top to bottom
                    leafPositions[i, j] = controlPoint.LeafPositions[i, n - j - 1];

            return leafPositions;
        }

        public double[] GetLeafWidths(Patient patient, PlanSetup plan, Beam beam)
        {
            try
            {
                return GetLeafWidthsFromAria(patient, plan, beam).ToArray();
            }
            catch (Exception e)
            {
                throw new LeafWidthsNotFoundException
                    ("Unable to obtain leaf widths for beam " + beam.Id, e);
            }
        }

        private IEnumerable<double> GetLeafWidthsFromAria(
            Patient patient, PlanSetup plan, Beam beam)
        {
            var course = plan.Course;

            using (var ac = new AriaContext())
            {
                // Use ESAPI IDs to get to the ARIA Radiation row
                var dbPatient = ac.Patients.First(p => p.PatientId == patient.Id);
                var dbCourse = dbPatient.Courses.First(c => c.CourseId == course.Id);
                var dbPlan = dbCourse.PlanSetups.First(ps => ps.PlanSetupId == plan.Id);
                var dbRad = dbPlan.Radiations.First(r => r.RadiationId == beam.Id);

                // Use the RadiationSer to get to the MLC add-on
                var dbFieldAddOns = ac.FieldAddOns.Where(f => f.RadiationSer == dbRad.RadiationSer);
                var dbMlcAddOn = dbFieldAddOns.First(f => f.AddOn.AddOnType == "MLC");

                // Use the MLC row to get to the leaves (and their width)
                // Note: We only need to use one of the MLC banks because
                // the leaf widths are the same between leaf pairs
                var dbMlc = dbMlcAddOn.AddOn.MLC;
                var dbMlcLeaves = dbMlc.MLCBanks.First().MLCLeaves;
                return dbMlcLeaves.Select(lf => lf.Width);
            }
        }
    }
}
