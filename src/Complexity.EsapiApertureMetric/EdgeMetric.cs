using System.Collections.Generic;
using System.Linq;
using Complexity.ApertureMetric;

namespace Complexity.EsapiApertureMetric
{
    // Subclass of ComplexityMetric that represents the edge metric
    public class EdgeMetric : ComplexityMetric
    {
        // Returns the unweighted edge metrics of a list of apertures
        protected override double[] CalculatePerAperture(IEnumerable<Aperture> apertures)
        {
            ApertureMetric.EdgeMetric metric = new ApertureMetric.EdgeMetric();
            return (from aperture in apertures
                    select metric.Calculate(aperture)).ToArray();
        }
    }
}