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
        public Func<Scenario, IDictionary<AccessPoint, int>, double> SimilarityAlgorithm { get; set; }

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
        /// Get a scenario for the requested preference.
        /// </summary>
        /// <param name="contextId">The requested preference id.</param>
        /// <param name="preferences">Similar contexts predicted.</param>
        /// <returns></returns>
        public Scenario ChangeContext(string contextId, out IEnumerable<ContextPreference> preferences)
        {
            // prediction



            preferences = null;
            return null;
        }

        /// <summary>
        /// Get the most suitable scenario based on the releaved signals.
        /// </summary>
        /// <param name="preferences">Similar contexts predicted with preference value.</param>
        /// <returns>The most suitable scenario or null.</returns>
        public Scenario Prediction(out IEnumerable<ContextPreference> preferences)
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
                preferences = null;
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
                preferences = null;
                return null;
            }

            // create map with preference id as key.
            var mapPreferences = new Dictionary<string, ContextPreference>();
            double bestScenarioSimilarity = -1.0; // min value will be 0.0
            Scenario bestScenario = null;

            foreach (var scenario in similarScenarios)
            {
                if(scenario.Features.Count == 0)
                {
                    // skip scenario
                    continue;
                }
                // result for scenario
                double scenarioSimilarity = SimilarityAlgorithm(scenario,mapSignals);

                // update preference similarity
                ContextPreference preference;
                if(mapPreferences.TryGetValue(scenario.ContextId.ToString(), out preference))
                {
                    if(scenarioSimilarity > preference.Value)
                    {
                        // new best scenario for the preference
                        preference.Value = scenarioSimilarity;
                    }
                }
                else
                {
                    // new suitable preference found
                    mapPreferences.Add(scenario.ContextId.ToString(), new ContextPreference
                    {
                        ContextId = scenario.ContextId.ToString(),
                        Value = scenarioSimilarity
                    });
                }

                // update best scenario
                if(scenarioSimilarity > bestScenarioSimilarity)
                {
                    bestScenario = scenario;
                    bestScenarioSimilarity = scenarioSimilarity;
                }
            }

            preferences = (bestScenario == null ? null : mapPreferences.Values);

            if(preferences != null)
            {
                double global = 0;
                foreach(var context in preferences)
                {
                    global += context.Value;
                }
                if(global > 0)
                {
                    foreach(var context in preferences)
                    {
                        context.Value /= global;
                    }
                }
            }

            return bestScenario;
        }



        public Localizer()
        {

        }

    }
}
