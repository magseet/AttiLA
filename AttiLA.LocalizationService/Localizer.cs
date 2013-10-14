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

                double deltaSimilarity2 = 0;
                double similarity2 = 0;
                double reliability2 = 0;

                foreach(var feature in scenario.Features)
                {
                    // Reliability represents the maximum similarity2
                    int evidence;
                    double featureSimilarity;

                    if(!mapSignals.TryGetValue(feature.Key.AP, out evidence))
                    {
                        featureSimilarity = 0;
                    }
                    else if (feature.Value.Variance == 0)
                    {
                        featureSimilarity = (evidence == feature.Value.Avg ? feature.Value.Reliability : 0);
                    }
                    else
                    {
                        double Z = evidence - feature.Value.Avg;
                        Z = Z * Z;
                        Z = -(Z / (2 * feature.Value.Variance * feature.Value.Variance));
                        featureSimilarity = feature.Value.Reliability * Math.Exp(Z);
                    }
                    //var featureDeltaSimilarity = feature.Value.Reliability - featureSimilarity;

                    similarity2 += featureSimilarity * featureSimilarity;
                    //deltaSimilarity2 += featureDeltaSimilarity * featureDeltaSimilarity;
                    reliability2 += feature.Value.Reliability * feature.Value.Reliability;

                }

                // result for scenario
                //double scenarioSimilarity = (similarity2 == 0 ? 0 : 1 - Math.Sqrt(deltaSimilarity2 / similarity2));
                //double scenarioSimilarity = (similarity2 == 0 ? 0 : 1 - Math.Sqrt(deltaSimilarity2 / reliability2));
                double scenarioSimilarity = (similarity2 == 0 ? 0 : Math.Sqrt(similarity2 / reliability2));

                // update context similarity2
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



    }
}
