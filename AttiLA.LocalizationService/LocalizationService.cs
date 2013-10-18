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
        /// The service state.
        /// </summary>
        public ServiceStateCode ServiceState { get; private set; }

        public double PredictionInterval { get; private set; }

        public uint NotificationThreshold { get; private set; }

        private uint ConsecutiveCorrectPredictions { get; set; }

        #endregion


        /// <summary>
        /// The tracking module.
        /// </summary>
        private Tracker tracker = new Tracker 
        { 
            Interval = Properties.Settings.Default.TrackerInterval,
            Enabled = false,
            ScenarioId = null
        };

        /// <summary>
        /// The localization module.
        /// </summary>
        private Localizer localizer = new Localizer
        {
            ContextId = null,
            CreationAllowed = false,
            Enabled = false,
            Interval = Properties.Settings.Default.LocalizerInterval,
            Retries = Properties.Settings.Default.LocalizerRetries,
            SimilarityAlgorithm = SimilarityAlgorithms.PredefinedAlgorithm(SimilarityAlgorithmCode.NaiveBayes)   
        };


        /// <summary>
        /// Service initialization.
        /// </summary>
        public LocalizationService()
        {
            // initialize properties
            ConsecutiveCorrectPredictions = 0;
            NotificationThreshold = Properties.Settings.Default.NotificationThreshold;
            PredictionInterval = Properties.Settings.Default.LocalizerInterval;
            ServiceState = ServiceStateCode.Idle;
            
            // enable handlers
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
                                subscriber.ReportLocalizationProgress(e.ProgressValue);
                            }
                            else
                            {
                                subscribers.Remove(subscriber);
                            }
                        }
                        break;

                    case LocalizerNotificationCode.Prediction:

                        // dispatch prediction
                        if(ServiceState == ServiceStateCode.Tracking)
                        {
                            tracker.Update();
                        }
                        DispatchPrediction(e.PredictionValue);
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
                                subscriber.ReportTracking(true, contextId);
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
                                subscriber.ReportTracking(false, contextId);
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
                return new ServiceStatus
                {
                    ContextId = localizer.ContextId,
                    ServiceState = this.ServiceState
                };
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
                    Interval = Properties.Settings.Default.LocalizerInterval,
                    Retries = Properties.Settings.Default.LocalizerRetries,
                    SimilarityAlgorithm = (SimilarityAlgorithmCode) Properties.Settings.Default.LocalizerSimilarityAlgorithm
                },
                Tracker = new TrackerSettings
                {
                    Interval = Properties.Settings.Default.TrackerInterval,
                },
                NotificationThreshold = Properties.Settings.Default.NotificationThreshold
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
            lock(lockStatus)
            {
                try
                {
                    // apply settings
                    tracker.Interval = newSettings.Tracker.Interval;
                    localizer.Interval = newSettings.Localizer.Interval;
                    localizer.SimilarityAlgorithm = SimilarityAlgorithms.PredefinedAlgorithm(newSettings.Localizer.SimilarityAlgorithm);
                    localizer.Retries = newSettings.Localizer.Retries;
                    NotificationThreshold = newSettings.NotificationThreshold;
                }
                catch
                {
                    // error applying settings
                    return false;
                }
                // save new settings
                Properties.Settings.Default.TrackerInterval = newSettings.Tracker.Interval;
                Properties.Settings.Default.LocalizerInterval = newSettings.Localizer.Interval;
                Properties.Settings.Default.LocalizerRetries = newSettings.Localizer.Retries;
                Properties.Settings.Default.LocalizerSimilarityAlgorithm = (byte)newSettings.Localizer.SimilarityAlgorithm;
                Properties.Settings.Default.NotificationThreshold = newSettings.NotificationThreshold;
                return true;
            }
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
                var detail = new ServiceException
                {
                    Code = ServiceExceptionCode.Arguments
                };
                throw new FaultException<ServiceException>(detail);
            }

            lock(lockStatus)
            {

                var previousState = ServiceState;

                try
                {
                    if(contextId == localizer.ContextId && previousState == ServiceStateCode.Tracking)
                    {
                        // do nothing
                        return true;
                    }

                    // prepare for tracking
                    tracker.ScenarioId = null;
                    localizer.Enabled = false;
                    ServiceState = ServiceStateCode.Tracking;

                    if(contextId != localizer.ContextId)
                    {
                        // prepare for new context
                        ConsecutiveCorrectPredictions = 0;
                        localizer.ContextId = contextId;
                    }

                    // perform a first prediction
                    localizer.CreationAllowed = true;
                    var prediction = localizer.Prediction();
                    if(prediction == null || prediction.PredictedScenario == null)
                    {
                        return false;
                    }
                    DispatchPrediction(prediction);
                    // enable localizer timer for the next predictions
                    localizer.Enabled = true;
                    
                }
                catch(ArgumentException)
                {
                    Silence();
                    var detail = new ServiceException
                    {
                        Code = ServiceExceptionCode.Arguments
                    };
                    throw new FaultException<ServiceException>(detail);
                }
                catch
                {
                    Silence();
                    return false;
                }

                return true;      
            }
        }

        /// <summary>
        /// Perform actions based on current status and prediction.
        /// </summary>
        /// <param name="prediction"></param>
        private void DispatchPrediction(PredictionArgs prediction)
        {
            if(prediction == null)
            {
                // do nothing
                return;
            }

            lock(lockStatus)
            {
                if(ServiceState == ServiceStateCode.Idle)
                {
                    // do nothing
                    return;
                }

                // update prediction counter
                var previousCounter = ConsecutiveCorrectPredictions;
                var currentCounter = (prediction.Success ? previousCounter + 1 : 0);
                ConsecutiveCorrectPredictions = currentCounter;

                if(
                    (previousCounter >= NotificationThreshold && currentCounter == 0) ||
                    (previousCounter < NotificationThreshold && currentCounter == NotificationThreshold))
                {
                    // notify all subscribers about prediction
                    foreach (var subscriber in subscribers)
                    {
                        if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                        {
                            var contextId = 
                                prediction.PredictedScenario == null 
                                ? null : prediction.PredictedScenario.ContextId.ToString();

                            subscriber.ReportPrediction(contextId);
                        }
                        else
                        {
                            subscribers.Remove(subscriber);
                        }
                    }

                }

                if(ServiceState == ServiceStateCode.Tracking)
                {
                    if(prediction.Success)
                    {
                        // no need to track
                        tracker.Enabled = false;
                    }
                    else
                    {
                        // track needed
                        tracker.ScenarioId = 
                            prediction.PredictedScenario == null
                            ? null : prediction.PredictedScenario.Id.ToString();
                        tracker.Enabled = true;
                    }
                }



            }

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
                localizer.CreationAllowed = false;
                if(ServiceState == ServiceStateCode.Tracking)
                {
                    ServiceState = ServiceStateCode.Notification;
                }
            }
            return true;
        }

        /// <summary>
        /// Implementation of service operation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ContextPreference> GetCloserContexts()
        {
            IEnumerable<ContextPreference> preferences;
            localizer.GetScenarioForCurrentPosition(out preferences);
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
            localizer.Enabled = false;

            lock(lockStatus)
            {
                ServiceState = ServiceStateCode.Idle;
            }
        }


        private ServiceStateCode SwapState(ServiceStateCode newState)
        {
            lock(lockStatus)
            {
                var previous = ServiceState;
                ServiceState = newState;
                return previous;
            }
        }
    }
}
