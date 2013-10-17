using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AttiLA.Data.Entities;

namespace AttiLA.LocalizationService
{
    internal class SimilarityAlgorithms
    {
        internal class Utils
        {
            internal static double Phi(double x)
            {
                // constants
                const double a1 = 0.254829592;
                const double a2 = -0.284496736;
                const double a3 = 1.421413741;
                const double a4 = -1.453152027;
                const double a5 = 1.061405429;
                const double p = 0.3275911;

                // Save the sign of x
                int sign = Math.Sign(x);
                x = Math.Abs(x) / Math.Sqrt(2.0);

                // A&S formula 7.1.26
                double t = 1.0 / (1.0 + p * x);
                double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

                return 0.5 * (1.0 + sign * y);
            }
        }

        internal static Func<Scenario,IDictionary<AccessPoint,int>,double> PredefinedAlgorithm(SimilarityAlgorithmCode name)
        {
            switch(name)
            {
                case SimilarityAlgorithmCode.NaiveBayes:
                    return delegate(Scenario scenario, IDictionary<AccessPoint, int> mapSignals)
                    {
                        double naiveSimilarity = 1;
                        foreach (var feature in scenario.Features)
                        {
                            // Reliability represents the maximum similarity2
                            int evidence;
                            double featureSimilarityWhenFound;
                            double featureSimilarityWhenNotFound;
                            double featureSimilarity;

                            if (!mapSignals.TryGetValue(feature.Key.AP, out evidence))
                            {
                                // no evidence for this feature
                                featureSimilarityWhenFound = 0;
                                featureSimilarityWhenNotFound = 1;
                            }
                            else if (feature.Value.Variance == 0)
                            {
                                // evidence found - feature is a delta function
                                featureSimilarityWhenFound = (evidence == feature.Value.Avg ? 1 : 0);
                                featureSimilarityWhenNotFound = (evidence < feature.Value.Avg ? 1 : 0);
                            }
                            else
                            {
                                // evindence found - feature is a gaussian function
                                double Z = evidence - feature.Value.Avg;
                                Z = Z * Z;
                                Z = -(Z / (2 * feature.Value.Variance * feature.Value.Variance));
                                featureSimilarityWhenFound = Math.Exp(Z);
                                featureSimilarityWhenNotFound = 1.0 - Utils.Phi((evidence - feature.Value.Avg) / feature.Value.Variance);
                            }
                            featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound + (1.0 - feature.Value.Reliability) * featureSimilarityWhenNotFound;
                            naiveSimilarity *= featureSimilarity;
                        }
                        return naiveSimilarity;
                    };

                /// -----------------------------------------------------------
                case SimilarityAlgorithmCode.RelativeErrorExtended:
                    return delegate(Scenario scenario, IDictionary<AccessPoint, int> mapSignals)
                    {
                        double similarity2 = 0;

                        foreach (var feature in scenario.Features)
                        {
                            // Reliability represents the maximum similarity2
                            int evidence;
                            double featureSimilarityWhenFound;
                            double featureSimilarityWhenNotFound;
                            double featureSimilarity;
                           
                            if (!mapSignals.TryGetValue(feature.Key.AP, out evidence))
                            {
                                // no evidence for this feature
                                featureSimilarityWhenFound = 0;
                                featureSimilarityWhenNotFound = 1;
                            }
                            else if (feature.Value.Variance == 0)
                            {
                                // evidence found - feature is a delta function
                                featureSimilarityWhenFound = (evidence == feature.Value.Avg ? 1 : 0);
                                featureSimilarityWhenNotFound = (evidence < feature.Value.Avg ? 1 : 0);
                            }
                            else
                            {
                                // evindence found - feature is a gaussian function
                                double Z = evidence - feature.Value.Avg;
                                Z = Z * Z;
                                Z = -(Z / (2 * feature.Value.Variance * feature.Value.Variance));
                                featureSimilarityWhenFound = Math.Exp(Z);
                                featureSimilarityWhenNotFound = 1.0 - Utils.Phi((evidence - feature.Value.Avg) / feature.Value.Variance);
                            }
                            featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound + (1.0 - feature.Value.Reliability) * featureSimilarityWhenNotFound;

                            similarity2 += featureSimilarity * featureSimilarity;
                        }

                        // result for scenario
                        double scenarioSimilarity = (similarity2 == 0 ? 0 : Math.Sqrt(similarity2 / scenario.Features.Count));
                        return scenarioSimilarity;

                    };
                /// -----------------------------------------------------------
                case SimilarityAlgorithmCode.RelativeError:
                    return delegate(Scenario scenario, IDictionary<AccessPoint, int> mapSignals)
                    {
                        double similarity2 = 0;
                        double maxSimilarity2 = 0;

                        foreach(var feature in scenario.Features)
                        {
                            // Reliability represents the maximum similarity2
                            int evidence;
                            double featureSimilarityWhenFound;
                            double featureSimilarity;
                            double featureMaxSimilarityWhenFound = 1;
                            //double featureMaxSimilarityWhenFound = (feature.Value.Variance == 0 ? 1 : 1 / Math.Sqrt(2 * Math.PI * feature.Value.Variance));
                            double featureMaxSimilarity = feature.Value.Reliability * featureMaxSimilarityWhenFound;

                            if(!mapSignals.TryGetValue(feature.Key.AP, out evidence))
                            {
                                // no evidence for this feature
                                featureSimilarityWhenFound = 0;
                            }
                            else if (feature.Value.Variance == 0)
                            {
                                // evidence found - feature is a delta function
                                featureSimilarityWhenFound = (evidence == feature.Value.Avg ? featureMaxSimilarityWhenFound : 0);
                            }
                            else
                            {
                                // evindence found - feature is a gaussian function
                                double Z = evidence - feature.Value.Avg;
                                Z = Z * Z;
                                Z = -(Z / (2 * feature.Value.Variance * feature.Value.Variance));
                                featureSimilarityWhenFound = featureMaxSimilarityWhenFound * Math.Exp(Z);
                            }
                            featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound;

                            similarity2 += featureSimilarity * featureSimilarity;
                            maxSimilarity2 += featureMaxSimilarity * featureMaxSimilarity;
                        }
                        // result for scenario
                        double scenarioSimilarity = (similarity2 == 0 ? 0 : Math.Sqrt(similarity2 / maxSimilarity2));
                        return scenarioSimilarity;
                    };
            }
            return null;
        }
        
    }
}
