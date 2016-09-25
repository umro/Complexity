using System.Collections.Generic;
using System.Linq;
using Complexity.ApertureMetric;
using VMS.TPS.Common.Model.API;

namespace Complexity.EsapiApertureMetric
{
    // Abstract class that represents any complexity metric;
    // it implements many common methods, but leaves
    // the actual metric calculation to subclasses
    public abstract class ComplexityMetric
    {
        #region Complexity metric for a plan

        // Returns the complexity metric of a plan, calculated as
        // the weighted sum of the individual metrics for each beam
        public double CalculateForPlan(PlanSetup plan)
        {
            return WeightedSum(GetWeights(plan), GetMetrics(plan));
        }

        // Returns the weights of a plan's beams;
        // by default, the weights are the meterset values per beam
        protected virtual double[] GetWeights(PlanSetup plan)
        {
            return GetMetersets(plan);
        }

        // Returns the total metersets of a plan's beams
        protected double[] GetMetersets(PlanSetup plan)
        {
            return (from beam in plan.Beams
                    where !beam.IsSetupField
                    select beam.Meterset.Value).ToArray();
        }

        // Returns the unweighted metrics of a plan's beams
        private double[] GetMetrics(PlanSetup plan)
        {
            return CalculateForPlanPerBeam(plan);
        }

        // Returns the unweighted metrics of a plan's non-setup beams
        private double[] CalculateForPlanPerBeam(PlanSetup plan)
        {
            return (from beam in plan.Beams
                    where !beam.IsSetupField
                    select CalculateForBeam(plan, beam)).ToArray();
        }

        #endregion // Complexity metric for a plan

        #region Complexity metric for a beam

        // Returns the complexity metric of a beam, calculated as
        // the weighted sum of the individual metrics for each control point
        public double CalculateForBeam(PlanSetup plan, Beam beam)
        {
            return WeightedSum(GetWeights(beam), GetMetrics(plan, beam));
        }

        // Returns the weights of a beam's control points;
        // by default, the weights are the meterset values per control point
        protected virtual double[] GetWeights(Beam beam)
        {
            return GetMetersets(beam);
        }

        // Returns the metersets of a beam's control points
        protected double[] GetMetersets(Beam beam)
        {
            return (new MetersetsFromMetersetWeightsCreator()).Create(beam);
        }

        // Returns the unweighted metrics of a beam's control points
        protected virtual double[] GetMetrics(PlanSetup plan, Beam beam)
        {
            return CalculateForBeamPerAperture(plan, beam);
        }

        // Returns the unweighted metrics of a beam's apertures
        private double[] CalculateForBeamPerAperture(PlanSetup plan, Beam beam)
        {
            return CalculatePerAperture(CreateApertures(plan, beam));
        }

        // Returns the unweighted metrics of a list of apertures;
        // it must be overridden by a subclass
        protected abstract double[] CalculatePerAperture(IEnumerable<Aperture> apertures);

        // Returns the apertures created from a beam
        private IEnumerable<Aperture> CreateApertures(PlanSetup plan, Beam beam)
        {
            return new AperturesFromBeamCreator().Create(plan, beam);
        }

        #endregion // Complexity metric for a beam

        #region Complexity metric for control points

        // Returns the weighted metrics of a beam's control points
        public double[] CalculatePerControlPointWeighted(PlanSetup plan, Beam beam)
        {
            return WeightedValues(GetWeights(beam), GetMetrics(plan, beam));
        }

        // Returns the unweighted metrics of a beam's control points
        public double[] CalculatePerControlPointUnweighted(PlanSetup plan, Beam beam)
        {
            return GetMetrics(plan, beam);
        }

        // Returns the weights of a beam's control points
        public double[] CalculatePerControlPointWeightsOnly(Beam beam)
        {
            return GetWeights(beam);
        }

        #endregion // Complexity metric for control points

        #region Helper methods

        // Returns the weighted sum of the given values and weights
        protected double WeightedSum(double[] weights, double[] values)
        {
            return WeightedValues(weights, values).Sum();
        }

        // Returns the normalized values of the given values and weights
        protected double[] WeightedValues(double[] weights, double[] values)
        {
            double weightSum = weights.Sum();
            return (from i in Enumerable.Range(0, values.Length)
                    select (weights[i] / weightSum) * values[i]).ToArray();
        }

        #endregion // Helper methods
    }
}