using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;
using AttiLA.Data;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using System.Timers;


namespace AttiLA.LocalizationService
{

    /// <summary>
    /// Service implementation:
    ///  --- Processes messages on one thread at a time
    ///  --- Creates a single service object
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single)]
    public class LocalizationService : ILocalizationService
    {

        #region Locks

        /// <summary>
        /// The lock used to synchronize access to the service status.
        /// </summary>
        private Object lockStatus = new Object();
        #endregion

        #region Events


        #endregion

        #region Static modules
        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private static readonly ScenarioService scenarioService = new ScenarioService();

        /// <summary>
        /// Service to interact with contexts in database.
        /// </summary>
        private static readonly ContextService contextService = new ContextService();

        /// <summary>
        /// Record the subscribers to the subscriber service.
        /// </summary>
        private static readonly List<ILocalizationServiceCallback>
            subscribers = new List<ILocalizationServiceCallback>();

        #endregion

        #region Properties

        /// <summary>
        /// The service status.
        /// </summary>
        public ServiceStatus Status { get; private set; }

        public double PredictionInterval { get; private set; }

        public uint NotificationThreshold { get; private set; }

        private uint ConsecutiveCorrectPredictions { get; set; }

        #endregion


        /// <summary>
        /// The tracking module.
        /// </summary>
        private Tracker tracker = new Tracker 
        { 
            CaptureInterval = Properties.Settings.Default.TrackerCaptureInterval,
            UpdateInterval = Properties.Settings.Default.TrackerUpdateInterval
        };

        /// <summary>
        /// The localization module.
        /// </summary>
        private Localizer localizer = new Localizer
        {
            Retries = Properties.Settings.Default.LocalizerRetries,
            SimilarityAlgorithm = SimilarityAlgorithms.PredefinedAlgorithm(SimilarityAlgorithmCode.NaiveBayes)
        };

        /// <summary>
        /// The timer used to perform predictions.
        /// </summary>
        private Timer predictionTimer = new Timer();

        /// <summary>
        /// Service initialization.
        /// </summary>
        public LocalizationService()
        {
            PredictionInterval = Properties.Settings.Default.PredictionInterval;
            NotificationThreshold = Properties.Settings.Default.NotificationThreshold;
            ConsecutiveCorrectPredictions = 0;

            Status = new ServiceStatus
            {
                ServiceState = ServiceStateCode.Idle
            };

            tracker.TrackerNotification += tracker_TrackerNotification;
            localizer.LocalizerNotification += localizer_LocalizerNotification;
        }

        /// <summary>
        /// Handler for notifications by the localizer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void localizer_LocalizerNotification(object sender, LocalizerNotificationEventArgs e)
        {
            // requires a lock since it's running on a different thread
            lock(lockStatus)
            {
                switch (e.Code)
                {
                    case LocalizerNotificationCode.Progress:
                        // notify all subscribers about localization progress
                        foreach (var subscriber in subscribers)
                        {
                            if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                            {
                                subscriber.ReportLocalizationProgress(e.ValueAsProgress);
                            }
                            else
                            {
                                subscribers.Remove(subscriber);
                            }
                        }
                        break;
                }

            }
        }

        /// <summary>
        /// Handler for notifications by the tracker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tracker_TrackerNotification(object sender, TrackerNotificationEventArgs e)
        {
            // requires a lock since it's running on a different thread
            lock(lockStatus)
            {
                switch (e.Code)
                {
                    case TrackerNotificationCode.NoSignalsDetected:
                        break;

                    case TrackerNotificationCode.Start:
                        // notify all subscribers about tracker start
                        var startTime = DateTime.Now;
                        foreach (var subscriber in subscribers)
                        {
                            if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                            {
                                var contextId = (e.TargetScenario == null ? null : e.TargetScenario.ContextId.ToString());
                                subscriber.TrackModeStarted(startTime, contextId);
                            }
                            else
                            {
                                subscribers.Remove(subscriber);
                            }
                        }
                        break;

                    case TrackerNotificationCode.Stop:
                        // notify all subscribers about tracker stop
                        var stopTime = DateTime.Now;
                        foreach (var subscriber in subscribers)
                        {
                            if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                            {
                                var contextId = (e.TargetScenario == null ? null : e.TargetScenario.ContextId.ToString());
                                subscriber.TrackModeStopped(stopTime, contextId);
                            }
                            else
                            {
                                subscribers.Remove(subscriber);
                            }
                        }
                        break;

                }

            }
        }


        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public bool Subscribe()
        {
            try
            {
                var callback = OperationContext.Current
                    .GetCallbackChannel<ILocalizationServiceCallback>();
                if(!subscribers.Contains(callback))
                {
                    subscribers.Add(callback);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public bool Unsubscribe()
        {
            try
            {
                var callback = OperationContext.Current
                    .GetCallbackChannel<ILocalizationServiceCallback>();
                if (subscribers.Contains(callback))
                    subscribers.Remove(callback);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public ServiceStatus GetServiceStatus()
        {
            lock(lockStatus)
            {
                return Status;
            }
        }


        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public GlobalSettings GetGlobalSettings()
        {
            return new GlobalSettings
            {
                Localizer = new LocalizerSettings
                {
                    Retries = Properties.Settings.Default.LocalizerRetries,
                    SimilarityAlgorithm = (SimilarityAlgorithmCode) Properties.Settings.Default.LocalizerSimilarityAlgorithm
                },
                Tracker = new TrackerSettings
                {
                    CaptureInterval = Properties.Settings.Default.TrackerCaptureInterval,
                    UpdateInterval = Properties.Settings.Default.TrackerUpdateInterval,
                }
            };
        }

        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <param name="newSettings"></param>
        /// <returns></returns>
        public bool SetGlobalSettings(GlobalSettings newSettings)
        {
            Silence();
            try
            {
                // apply settings
                tracker.CaptureInterval = newSettings.Tracker.CaptureInterval;
                tracker.UpdateInterval = newSettings.Tracker.UpdateInterval;
                localizer.SimilarityAlgorithm = SimilarityAlgorithms.PredefinedAlgorithm(newSettings.Localizer.SimilarityAlgorithm);
                localizer.Retries = newSettings.Localizer.Retries;
            }
            catch
            {
                // error applying settings
                return false;
            }
            // save new settings
            Properties.Settings.Default.TrackerCaptureInterval = newSettings.Tracker.CaptureInterval;
            Properties.Settings.Default.TrackerUpdateInterval = newSettings.Tracker.UpdateInterval;
            Properties.Settings.Default.LocalizerRetries = newSettings.Localizer.Retries;
            Properties.Settings.Default.LocalizerSimilarityAlgorithm = (byte)newSettings.Localizer.SimilarityAlgorithm;
            return true;
        }


        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public bool TrackingStart(string contextId)
        {
            if (contextId == null || !ContextService.IsValidObjectID(contextId))
            {
                // TODO: correct here..
                throw new FaultException<ArgumentException>(
                    new ArgumentException("contextId"));
            }

            IEnumerable<ContextPreference> preferences;
            var scenario = localizer.ChangeContext(contextId, out preferences);
            if (scenario == null)
            {
                // TODO..
                return false;
            }

            tracker.ScenarioId = scenario.Id.ToString();

            tracker.Enabled = true;

            return true;
        }

        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public bool TrackingStop()
        {
            tracker.Enabled = false;
            lock(lockStatus)
            {
                if(Status.ServiceState == ServiceStateCode.Tracking)
                {
                    Status.ServiceState = ServiceStateCode.Notification;
                }
            }
            return true;
        }

        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContextPreference> Prediction()
        {
            IEnumerable<ContextPreference> preferences;
            localizer.Prediction(out preferences);
            if (preferences == null)
            {
                return new List<ContextPreference>();
            }
            return preferences;
        }

        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public void Silence()
        {
            tracker.Enabled = false;
            lock(lockStatus)
            {
                Status.ServiceState = ServiceStateCode.Idle;
            }
        }
    }
}
