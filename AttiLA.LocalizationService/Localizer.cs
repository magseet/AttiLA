using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;

namespace AttiLA.LocalizationService
{
    public class Localizer
    {

        public Func<IEnumerable<Feature>, IDictionary<AccessPoint, int>, double> SimilarityAlgorithm { get; set; }


        private static readonly Dictionary<SimilarityAlgorithmType, Func<IEnumerable<Feature>, IDictionary<AccessPoint, int>, double>>
            algorithms = new Dictionary<SimilarityAlgorithmType, Func<IEnumerable<Feature>, IDictionary<AccessPoint, int>, double>>();

        /// <summary>
        /// Samples supplier module.
        /// </summary>
        private WlanScanner wlanScanner = WlanScanner.Instance;

        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private ScenarioService scenarioService = new ScenarioService();

        private int retries;

        /// <summary>
        /// Number of retries on WLAN scan failure.
        /// </summary>
        public int Retries {
            get { return retries; }
            set
            {
                retries = (value > 0 ? value : 0);
            }
        }

        /// <summary>
        /// Get a scenario for the requested context.
        /// </summary>
        /// <param name="contextId">The requested context id.</param>
        /// <param name="similarContexts">Similar contexts predicted.</param>
        /// <returns></returns>
        public Scenario ChangeContext(string contextId, out IEnumerable<ContextSimilarity> similarContexts)
        {
            // prediction



            similarContexts = null;
            return null;
        }

        /// <summary>
        /// Get the most suitable scenario based on the releaved signals.
        /// </summary>
        /// <param name="similarContexts">Similar contexts predicted.</param>
        /// <returns>The most suitable scenario or null.</returns>
        public Scenario Prediction(out IEnumerable<ContextSimilarity> similarContexts)
        {
            List<ScanSignal> signals = null;
            int retries = this.Retries;

            for (var attempts = this.Retries + 1; attempts > 0; attempts--)
            {
                signals = wlanScanner.GetScanSignals();
                if(signals.Count > 0)
                {
                    break;
                }
            }

            if (signals.Count == 0)
            {
                similarContexts = null;
                return null;
            }


            // create map of signals for searches
            var mapSignals = new Dictionary<AccessPoint, int>();
            foreach (var signal in signals)
            {
                mapSignals.Add(signal.AP, signal.RSSI);
            }

            var similarScenarios = scenarioService.GetByPossibleAccessPoints(mapSignals.Keys);

            if(similarScenarios.Count() == 0)
            {
                // no suitable scenarios were found.
                similarContexts = null;
                return null;
            }

            // create map with context id as key.
            var mapContextSimilarities = new Dictionary<string, ContextSimilarity>();
            double bestScenarioSimilarity = -1.0; // min value will be 0.0
            Scenario bestScenario = null;

            foreach (var scenario in similarScenarios)
            {
                if(scenario.Features.Count == 0)
                {
                    // skip scenario
                    continue;
                }

                double similarity2 = 0;
                double reliability2 = 0;
                double naiveSimilarity = 1;

                foreach(var feature in scenario.Features)
                {
                    // Reliability represents the maximum similarity2
                    int evidence;
                    double featureSimilarityWhenFound;
                    double featureSimilarityWhenNotFound;
                    double featureSimilarity;
                    //double featureMaxSimilarity = feature.Value.Reliability * featureMaxSimilarityWhenFound;      // consider only first addend

                    if(!mapSignals.TryGetValue(feature.Key.AP, out evidence))
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
                        featureSimilarityWhenNotFound = 1.0 - Phi((evidence - feature.Value.Avg) / feature.Value.Variance);
                    }
                    featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound + (1.0 - feature.Value.Reliability) * featureSimilarityWhenNotFound;
                    //featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound;   // consider only first addend

                    similarity2 += featureSimilarity * featureSimilarity;
                    //reliability2 += featureMaxSimilarity * featureMaxSimilarity;
                    reliability2++;

                    naiveSimilarity *= featureSimilarity;
                }

                // result for scenario
                //double scenarioSimilarity = (similarity2 == 0 ? 0 : Math.Sqrt(similarity2 / reliability2));
                double scenarioSimilarity = naiveSimilarity;

                // update context similarity
                ContextSimilarity context;
                if(mapContextSimilarities.TryGetValue(scenario.ContextId.ToString(), out context))
                {
                    if(scenarioSimilarity > context.Similarity)
                    {
                        // new best scenario for the context
                        context.Similarity = scenarioSimilarity;
                    }
                }
                else
                {
                    // new suitable context found
                    mapContextSimilarities.Add(scenario.ContextId.ToString(), new ContextSimilarity
                    {
                        ContextId = scenario.ContextId.ToString(),
                        Similarity = scenarioSimilarity
                    });
                }

                // update best scenario
                if(scenarioSimilarity > bestScenarioSimilarity)
                {
                    bestScenario = scenario;
                    bestScenarioSimilarity = scenarioSimilarity;
                }
            }

            similarContexts = (bestScenario == null ? null : mapContextSimilarities.Values);
            return bestScenario;
        }



        public Localizer()
        {

        }

        private static double Phi(double x)
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

        private static double NaiveBayesSimilarity(IEnumerable<Feature> features, IDictionary<AccessPoint, int> mapSignals)
        {
            double naiveSimilarity = 1;
            foreach(var feature in features)
            {
                // Reliability represents the maximum similarity2
                int evidence;
                double featureSimilarityWhenFound;
                double featureSimilarityWhenNotFound;
                double featureSimilarity;

                if(!mapSignals.TryGetValue(feature.Key.AP, out evidence))
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
                    featureSimilarityWhenNotFound = 1.0 - Phi((evidence - feature.Value.Avg) / feature.Value.Variance);
                }
                featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound + (1.0 - feature.Value.Reliability) * featureSimilarityWhenNotFound;
                naiveSimilarity *= featureSimilarity;
            }
            return naiveSimilarity;
        }

        private static double RelativeErrorSimilarity(IEnumerable<Feature> features, IDictionary<AccessPoint, int> mapSignals)
        {
            double similarity2 = 0;
            double reliability2 = 0;
            double naiveSimilarity = 1;

            foreach (var feature in features)
            {
                // Reliability represents the maximum similarity2
                int evidence;
                double featureSimilarityWhenFound;
                double featureSimilarityWhenNotFound;
                double featureSimilarity;
                //double featureMaxSimilarity = feature.Value.Reliability * featureMaxSimilarityWhenFound;      // consider only first addend

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
                    featureSimilarityWhenNotFound = 1.0 - Phi((evidence - feature.Value.Avg) / feature.Value.Variance);
                }
                featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound + (1.0 - feature.Value.Reliability) * featureSimilarityWhenNotFound;
                //featureSimilarity = feature.Value.Reliability * featureSimilarityWhenFound;   // consider only first addend

                similarity2 += featureSimilarity * featureSimilarity;
                //reliability2 += featureMaxSimilarity * featureMaxSimilarity;
                reliability2++;

                naiveSimilarity *= featureSimilarity;
            }

            // result for scenario
            //double scenarioSimilarity = (similarity2 == 0 ? 0 : Math.Sqrt(similarity2 / reliability2));
            double scenarioSimilarity = naiveSimilarity;
            return scenarioSimilarity;
        }


    }
}
