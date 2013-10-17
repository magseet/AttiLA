using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;
using MongoDB.Bson;

namespace AttiLA.LocalizationService
{
    #region Notification classes
    /// <summary>
    /// The localizer notification codes.
    /// </summary>
    public enum LocalizerNotificationCode
    {
        Progress
    }

    /// <summary>
    /// Arguments of localizer notifications.
    /// </summary>
    public class LocalizerNotificationEventArgs : EventArgs
    {
        private object value;

        public LocalizerNotificationEventArgs(LocalizerNotificationCode code, object value = null)
        {
            Code = code;

            switch(code)
            {
                case LocalizerNotificationCode.Progress:
                    this.value = (double)value;
                    break;
            }
        }

        /// <summary>
        /// Value for progress notification event.
        /// </summary>
        public double ValueAsProgress
        {
            get
            {
                return (double)value;
            }

            set
            {
                this.value = (double)value;
            }
        }

        /// <summary>
        /// A code to identify the notification type.
        /// </summary>
        public LocalizerNotificationCode Code { get; set; }

    }

    /// <summary>
    /// The localizer notification error codes.
    /// </summary>
    public enum LocalizerErrorNotificationCode
    {
        DatabaseError,
        UnknownContext
    }


    /// <summary>
    /// Data for localizer error notification event handler.
    /// </summary>
    public class LocalizerErrorNotificationEventArgs : EventArgs
    {
        public LocalizerErrorNotificationEventArgs(LocalizerErrorNotificationCode code)
        {
            Code = code;
        }

        public LocalizerErrorNotificationEventArgs(LocalizerErrorNotificationCode code, Exception cause)
        {
            Code = code;
            Cause = cause;
        }

        /// <summary>
        /// A code to identify the localizer error notification type.
        /// </summary>
        public LocalizerErrorNotificationCode Code { get; set; }

        /// <summary>
        /// The exception that raised this localizer error.
        /// </summary>
        public Exception Cause { get; set; }
    }

    #endregion

    public class Localizer
    {
        #region Events
        /// <summary>
        /// Represents a method that will handle <see cref="LocalizerNotificationEvent"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LocalizerNotificationEventHandler(object sender, LocalizerNotificationEventArgs e);

        /// <summary>
        /// Represents a method that will handle <see cref="LocalizerErrorNotificationEvent"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void LocalizerErrorNotificationEventHandler(object sender, LocalizerErrorNotificationEventArgs e);

        /// <summary>
        /// Localizer notification event.
        /// </summary>
        public event LocalizerNotificationEventHandler LocalizerNotification;

        /// <summary>
        /// Localizer error notification event.
        /// </summary>
        public event LocalizerErrorNotificationEventHandler LocalizerErrorNotification;
        #endregion

        #region Properties
        /// <summary>
        /// Similarity algorithm used to perform matching between a scenario and signal example.
        /// </summary>
        public Func<Scenario, IDictionary<AccessPoint, int>, double> SimilarityAlgorithm
        {
            private get
            {
                lock(localizerLock)
                {
                    return 
                        similarityAlgorithm != null 
                        ? similarityAlgorithm 
                        : (scenario, signals) => 0.0;
                }
            }
            set
            {
                lock(localizerLock)
                {
                    similarityAlgorithm = value;
                }
            }
        }

        /// <summary>
        /// Number of retries on WLAN scan failure.
        /// </summary>
        public uint Retries
        {
            get
            {
                lock (localizerLock)
                {
                    return retries;
                }
            }
            set
            {
                lock(localizerLock)
                {
                    retries = (value > 0 ? value : 0);
                }
            }
        }

        #endregion

        /// <summary>
        /// The lock used to synchronize access to the localizer.
        /// </summary>
        private Object localizerLock = new Object();

        private Func<Scenario, IDictionary<AccessPoint, int>, double> similarityAlgorithm;


        /// <summary>
        /// Samples supplier module.
        /// </summary>
        private WlanScanner wlanScanner = WlanScanner.Instance;

        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private ScenarioService scenarioService = new ScenarioService();

        private uint retries;

        /// <summary>
        /// Get a scenario for the requested preference.
        /// </summary>
        /// <param name="contextId">The requested preference id.</param>
        /// <param name="preferences">Similar contexts predicted.</param>
        /// <returns></returns>
        public Scenario ChangeContext(string contextId, out IEnumerable<ContextPreference> preferences)
        {
            if(contextId == null)
            {
                // don't complain, should have been checked
                throw new ArgumentNullException("contextId");
            }

            if (!ContextService.IsValidObjectID(contextId))
            {
                // don't complain, should have been checked
                throw new ArgumentOutOfRangeException("contextId");
            }

            try
            {
                if (scenarioService.GetById(contextId) == null)
                {
                    // notify error
                    if(LocalizerErrorNotification != null)
                    {
                        var args = new LocalizerErrorNotificationEventArgs(
                            LocalizerErrorNotificationCode.UnknownContext);
                        LocalizerErrorNotification(this, args);
                    }
                    preferences = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                if(LocalizerErrorNotification != null)
                {
                    // notify error
                    var args = new LocalizerErrorNotificationEventArgs(
                        LocalizerErrorNotificationCode.DatabaseError,ex);
                    LocalizerErrorNotification(this, args);
                }
                preferences = null;
                return null;
            }

            // context is valid

            lock(localizerLock)
            {
                Scenario scenario = null;
                IEnumerable<ContextPreference> pref = null;
                for (var attempts = this.Retries + 1; attempts > 0; attempts--)
                {
                    
                    scenario = Prediction(out pref);
                    if (scenario == null)
                    {
                        // retry, eventually..
                        continue;
                    }

                    if(contextId.Equals(scenario.ContextId.ToString()))
                    {
                        // right prediction
                        scenarioService.IncreaseAccuracy(scenario);
                        break;
                    }
                    
                    // wrong prediction
                    scenarioService.DecreaseAccuracy(scenario);
                    scenario = null;
                }
                
                if(scenario == null)
                {
                    // create a new scenario for the requested context
                    scenario = new Scenario
                    {
                        ContextId = new ObjectId(contextId),
                        CreationTime = DateTime.Now
                    };

                    try
                    {
                        scenarioService.Create(scenario);
                    }
                    catch(Exception ex)
                    {
                        if(LocalizerErrorNotification != null)
                        {
                            // notify error
                            var args = new LocalizerErrorNotificationEventArgs(
                                LocalizerErrorNotificationCode.DatabaseError, ex);
                            LocalizerErrorNotification(this, args);
                        }
                        preferences = null;
                        return null;
                    }
                }

                // TODO: last preferences are applied for now.
                // Consider a better alternative, if any.
                preferences = pref;
                return null;

            }
        }

        /// <summary>
        /// Get the most suitable scenario based on the releaved signals.
        /// </summary>
        /// <param name="preferences">Similar contexts predicted with preference value.</param>
        /// <returns>The most suitable scenario or null.</returns>
        public Scenario Prediction(out IEnumerable<ContextPreference> preferences)
        {
            lock(localizerLock)
            {
                List<ScanSignal> signals = null;

                for (var attempts = this.Retries + 1; attempts > 0; attempts--)
                {
                    signals = wlanScanner.GetScanSignals();
                    if (signals.Count > 0)
                    {
                        break;
                    }
                }

                if (signals.Count == 0)
                {
                    // send progress notification 
                    if(LocalizerNotification != null)
                    {
                        var args = new LocalizerNotificationEventArgs(
                            LocalizerNotificationCode.Progress, 1.0);
                        LocalizerNotification(this, args);
                    }
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

                var numScenarios = similarScenarios.Count();
                if (numScenarios == 0)
                {
                    // no suitable scenarios were found.

                    // send progress notification 
                    if (LocalizerNotification != null)
                    {
                        var args = new LocalizerNotificationEventArgs(
                            LocalizerNotificationCode.Progress, 1.0);
                        LocalizerNotification(this, args);
                    }
                    preferences = null;
                    return null;
                }

                // create map with preference id as key.
                var mapPreferences = new Dictionary<string, ContextPreference>();
                double bestScenarioSimilarity = -1.0; // min value will be 0.0
                Scenario bestScenario = null;

                // index loop suitable for progress notification
                var scenarioEnumerator = similarScenarios.GetEnumerator();
                scenarioEnumerator.MoveNext();
                for (int scenarioCounter = 0; scenarioCounter < numScenarios; scenarioCounter++, scenarioEnumerator.MoveNext())
                {
                    // send progress notification 
                    if (LocalizerNotification != null)
                    {
                        var args = new LocalizerNotificationEventArgs(
                            LocalizerNotificationCode.Progress, (double) scenarioCounter / numScenarios);
                        LocalizerNotification(this, args);
                    }
                    var scenario = scenarioEnumerator.Current;
                    if (scenario.Features.Count == 0)
                    {
                        // skip scenario
                        continue;
                    }
                    // result for scenario
                    double scenarioSimilarity = SimilarityAlgorithm(scenario, mapSignals);

                    // update preference similarity
                    ContextPreference preference;
                    if (mapPreferences.TryGetValue(scenario.ContextId.ToString(), out preference))
                    {
                        if (scenarioSimilarity > preference.Value)
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
                    if (scenarioSimilarity > bestScenarioSimilarity)
                    {
                        bestScenario = scenario;
                        bestScenarioSimilarity = scenarioSimilarity;
                    }
                }
                scenarioEnumerator.Dispose();

                // send progress notification 
                if (LocalizerNotification != null)
                {
                    var args = new LocalizerNotificationEventArgs(
                            LocalizerNotificationCode.Progress, 1.0);
                    LocalizerNotification(this, args);
                }


                preferences = (bestScenario == null ? null : mapPreferences.Values);

                if (preferences != null)
                {
                    double global = 0;
                    foreach (var context in preferences)
                    {
                        global += context.Value;
                    }
                    if (global > 0)
                    {
                        foreach (var context in preferences)
                        {
                            context.Value /= global;
                        }
                    }
                }

                return bestScenario;

            }
        }



        public Localizer()
        {

        }

    }
}
