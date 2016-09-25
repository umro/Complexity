using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Complexity
{
    public class PatientData
    {
        public IList<PatientDataItem> Data { get; set; }

        public PatientData(string filePath)
        {
            try
            {
                Data = (from line in File.ReadAllLines(filePath)
                        let datum = new PatientDataItem(line)
                        select datum).ToList();
            }
            catch
            {
                Data = new List<PatientDataItem>();

                // Do nothing, fail silently
                // TODO: Put the try on whoever is creating PatientData,
                // and have the caller inform the user that an error happened
            }
        }

        public string[] GetUniqueSites()
        {
            return (from data in Data
                    select data.Site).Distinct().ToArray();
        }

        public double[] GetPlanMetrics(string site = null)
        {
            return (from data in Data
                    where site == null || data.Site == site
                    select data.PlanMetric).ToArray();
        }

        public double[] GetFieldMetrics(string site = null)
        {
            List<double> metrics = new List<double>();

            foreach (PatientDataItem data in Data)
            {
                if (site == null || data.Site == site)
                {
                    foreach (double metric in data.FieldMetrics)
                    {
                        metrics.Add(metric);
                    }
                }
            }

            return metrics.ToArray();
        }
    }
}
