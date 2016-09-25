using System;
using System.Linq;

namespace Complexity
{
    public class PatientDataItem
    {
        public string Site { get; set; }
        public double PlanMetric { get; set; }
        public double[] FieldMetrics { get; set; }

        public PatientDataItem(string csv)
        {
            string[] tokens = (from token in csv.Split(',')
                               where token.Length > 0
                               select token).ToArray();

            Site = Capitalize(tokens[0]);
            PlanMetric = Convert.ToDouble(tokens[1]);
            FieldMetrics = new double[tokens.Length - 2];
            for (int i = 2; i < tokens.Length; i++)
            {
                FieldMetrics[i - 2] = Convert.ToDouble(tokens[i]);
            }
        }

        // Capitalize the first letter of the given word
        private string Capitalize(string word)
        {
            if (word.Length > 1)
            {
                return word.Substring(0, 1).ToUpper() +
                    word.Substring(1, word.Length - 1);
            }
            else if (word.Length == 1)
            {
                return word.Substring(0, 1).ToUpper();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
