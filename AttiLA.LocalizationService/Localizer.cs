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
            var signals = wlanScanner.GetScanSignals();


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

                double deltaSimilarity = 0;
                double similarity = 0;

                foreach(var feature in scenario.Features)
                {
                    // Reliability represents the maximum similarity
                    var evidence = mapSignals[feature.Key.AP];
                    double featureSimilarity;
                    if (feature.Value.Variance == 0)
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
                    var featureDeltaSimilarity = feature.Value.Reliability - featureSimilarity;

                    similarity += featureSimilarity * featureSimilarity;
                    deltaSimilarity += featureDeltaSimilarity * featureDeltaSimilarity;

                }

                // result for scenario
                double scenarioSimilarity = (similarity == 0 ? 0 : 1 - Math.Sqrt(deltaSimilarity / similarity));

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



    }
}
