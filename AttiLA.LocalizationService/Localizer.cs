using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;
using MongoDB.Bson;
using System.Timers;
using System.Threading;
using System.Collections.Concurrent;

namespace AttiLA.LocalizationService
{
    #region Notification classes
    /// <summary>
    /// The localizer notification codes.
    /// </summary>
    public enum LocalizerNotificationCode
    {
        Progress,
        Prediction
    }

    /// <summary>
    /// Arguments of localizer notifications.
    /// </summary>
    public class LocalizerNotificationEventArgs : EventArgs
    {
        private object value;

        /// <summary>
        /// Value casting for progress notification event.
        /// </summary>
        public double ProgressValue
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
        /// Value casting for prediction notfication event.
        /// </summary>
        public PredictionArgs PredictionValue
        {
            get
            {
                return (PredictionArgs)value;
            }

            set
            {
                this.value = (PredictionArgs)value;
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
        UnknownContext,
        Prediction
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

    /// <summary>
    /// Data used in prediction notification event.
    /// </summary>
    public class PredictionArgs
    {
        public Scenario PredictedScenario { get; set; }
        public Boolean Success { get; set; }
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

        /// <summary>
        /// The lock used to synchronize access to the localizer.
        /// </summary>
        private Object localizerLock = new Object();

        private string contextId;

        private Func<Scenario, IDictionary<AccessPoint, int>, double> similarityAlgorithm;

        private bool creationAllowed;

        /// <summary>
        /// Samples supplier module.
        /// </summary>
        private WlanScanner wlanScanner = WlanScanner.Instance;

        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private ScenarioService scenarioService = new ScenarioService();

        /// <summary>
        /// Service to interact woth contexts in database.
        /// </summary>
        private ContextService contextService = new ContextService();

        private uint retries;

        /// <summary>
        /// The timer used to perform predictions.
        /// </summary>
        private System.Timers.Timer localizerTimer = new System.Timers.Timer();


        /// <summary>
        /// The collection used to store the progress values until they are notified.
        /// </summary>
        BlockingCollection<double> progressCollection = new BlockingCollection<double>();
        Task progressTask;

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
        /// Number of retries on failure.
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

        /// <summary>
        /// The context set by the user.
        /// </summary>
        public string ContextId
        {
            get
            {
                lock(localizerLock)
                {
                    return contextId;
                }
            }
            set
            {
                lock(localizerLock)
                {
                    if (value != null && !ContextService.IsValidObjectID(value))
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }
                    contextId = value;
                }
            }
        }

        /// <summary>
        /// The interval in milliseconds between predictions.
        /// </summary>
        public double Interval
        {
            get
            {
                lock (localizerLock)
                {
                    return localizerTimer.Interval;
                }
            }
            set
            {
                lock (localizerTimer)
                {
                    localizerTimer.Interval = value;
                }
            }
        }

        /// <summary>
        /// Property used to enable/disable the tracker.
        /// </summary>
        public bool Enabled
        {
            get
            {
                lock (localizerLock)
                {
                    return localizerTimer.Enabled;
                }
            }
            set
            {
                lock (localizerLock)
                {
                    localizerTimer.Enabled = value;
                }
            }
        }

        /// <summary>
        /// The localizer is allowed to create a new scenario on failure.
        /// </summary>
        public bool CreationAllowed
        {
            get
            {
                lock(localizerTimer)
                {
                    return creationAllowed;
                }
            }

            set
            {
                lock(localizerTimer)
                {
                    creationAllowed = value;
                }
            }
        }

        #endregion


        /// <summary>
        /// Each time this handler is invoked, a new prediction is done.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void localizerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {   
            lock(localizerLock)
            {
                // suspend
                localizerTimer.Stop();

                if (LocalizerNotification != null)
                {
                    var prediction = this.Prediction();
                    if (prediction != null)
                    {
                        var args = new LocalizerNotificationEventArgs
                        {
                            Code = LocalizerNotificationCode.Prediction,
                            PredictionValue = prediction
                        };
                        var t = new Thread(() => LocalizerNotification(this, args));
                        t.Start();
                    }

                }

                // resume
                localizerTimer.Start();
            }
        }

        /// <summary>
        /// Try to find a scenario for the target context. On failure, a new
        /// scenario is created if the creation is allowed, otherwise the
        /// predicted scenario is returned.
        /// </summary>
        /// <returns>A prediction, or null in case of error.</returns>
        public PredictionArgs Prediction()
        {
            lock (localizerLock)
            {

                if (ContextId == null)
                {
                    return null;
                }

                try
                {
                    if (contextService.GetById(ContextId) == null)
                    {
                        // notify error
                        if (LocalizerErrorNotification != null)
                        {
                            var args = new LocalizerErrorNotificationEventArgs(
                                LocalizerErrorNotificationCode.UnknownContext);
                            var t = new Thread(() => LocalizerErrorNotification(this, args));
                            t.Start();
                        }
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    if (LocalizerErrorNotification != null)
                    {
                        // notify error
                        var args = new LocalizerErrorNotificationEventArgs(
                            LocalizerErrorNotificationCode.DatabaseError, ex);
                        var t = new Thread(() => LocalizerErrorNotification(this, args));
                        t.Start();
                    }
                    return null;
                }

                // context found in the database

                var prediction = new PredictionArgs
                {
                    Success = false
                };

                IEnumerable<ContextPreference> pref = null;
                for (var attempts = this.Retries + 1; attempts > 0; attempts--)
                {

                    prediction.PredictedScenario = GetScenarioForCurrentPosition(out pref);
                    if (prediction.PredictedScenario == null)
                    {
                        // retry, eventually..
                        continue;
                    }

                    if (ContextId.Equals(prediction.PredictedScenario.ContextId.ToString()))
                    {
                        // correct prediction
                        scenarioService.IncreaseAccuracy(prediction.PredictedScenario);
                        prediction.Success = true;
                        break;
                    }

                    // wrong prediction
                    scenarioService.DecreaseAccuracy(prediction.PredictedScenario);
                }

                if (!prediction.Success && CreationAllowed)
                {
                    // create a new scenario for the requested context
                    prediction.PredictedScenario = new Scenario
                    {
                        ContextId = new ObjectId(ContextId),
                        CreationTime = DateTime.Now
                    };

                    try
                    {
                        scenarioService.Create(prediction.PredictedScenario);
                    }
                    catch (Exception ex)
                    {
                        if (LocalizerErrorNotification != null)
                        {
                            
                            // notify error
                            var args = new LocalizerErrorNotificationEventArgs(
                                LocalizerErrorNotificationCode.DatabaseError, ex);
                            var t = new Thread(() => LocalizerErrorNotification(this, args));
                            t.Start();
                        }
                        return null;
                    }
                }

                return prediction;

            }

        }

        /// <summary>
        /// Get the most suitable scenario based on the releaved signals.
        /// </summary>
        /// <param name="preferences">Similar contexts predicted with preference value.</param>
        /// <returns>The most suitable scenario or null.</returns>
        public Scenario GetScenarioForCurrentPosition(out IEnumerable<ContextPreference> preferences)
        {
            lock(localizerLock)
            {
                List<ScanSignal> signals = wlanScanner.GetScanSignals();

                if (signals.Count == 0)
                {
                    progressCollection.Add(1.0);
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
                    progressCollection.Add(1.0);
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
                    progressCollection.Add((double)scenarioCounter / numScenarios);

                    var scenario = scenarioEnumerator.Current;
                    if (scenario.Features.Count == 0)
                    {
                        // skip scenario
                        continue;
                    }
                    // prediction for scenario
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
                progressCollection.Add(1.0);

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
            localizerTimer.Elapsed += localizerTimer_Elapsed;

            // task to notify localization progress
            progressTask = Task.Factory.StartNew(() =>
            {
                foreach (double progress in progressCollection.GetConsumingEnumerable())
                {
                    if(LocalizerNotification != null)
                    {
                        var args = new LocalizerNotificationEventArgs
                        {
                            Code = LocalizerNotificationCode.Progress,
                            ProgressValue = progress
                        };
                        LocalizerNotification(this, args);
                    }
                }
            });
        }





        
    }
}
