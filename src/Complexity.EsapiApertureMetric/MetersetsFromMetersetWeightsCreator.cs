using System.Linq;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Complexity.EsapiApertureMetric
{
    class MetersetsFromMetersetWeightsCreator
    {
        public double[] Create(Beam beam)
        {
            if (beam.Meterset.Unit != DosimeterUnit.MU)
                return null;

            double[] metersetWeights = GetMetersetWeights(beam.ControlPoints);
            double[] metersets = ConvertMetersetWeightsToMetersets
                (beam.Meterset.Value, metersetWeights);

            return UndoCummulativeSum(metersets);
        }

        private static double[] GetMetersetWeights(ControlPointCollection controlPoints)
        {
            return controlPoints.Select(x => x.MetersetWeight).ToArray();
        }

        private static double[] ConvertMetersetWeightsToMetersets
            (double beamMeterset, double[] metersetWeights)
        {
            double finalMetersetWeight = metersetWeights[metersetWeights.Length - 1];
            return metersetWeights.Select(x => beamMeterset * x / finalMetersetWeight).ToArray();
        }

        // Returns the values whose cummulative sum is "cummulativeSum"

        private static double[] UndoCummulativeSum(double[] cummulativeSum)
        {
            double[] values = new double[cummulativeSum.Length];

            double delta_prev = 0.0;

            for (int i = 0; i < values.Length - 1; i++)
            {
                double delta_curr = cummulativeSum[i + 1] - cummulativeSum[i];
                values[i] = 0.5 * delta_prev + 0.5 * delta_curr;
                delta_prev = delta_curr;
            }

            values[values.Length - 1] = 0.5 * delta_prev;

            return values;
        }
    }
}
