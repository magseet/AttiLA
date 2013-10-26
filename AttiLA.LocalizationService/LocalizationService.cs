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
using System.Diagnostics;

namespace AttiLA.LocalizationService
{

    /// <summary>
    /// Service implementation:
    ///  --- Processes messages on one thread at a time
    ///  --- Creates a single service object
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LocalizationService : ILocalizationService
    {

        #region Events


        #endregion

        #region Static members
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

        #region Private members

        /// <summary>
        /// The lock used to synchronize access to the service status.
        /// </summary>
        private Object _lockStatus = new Object();

        /// <summary>
        /// The tracking module.
        /// </summary>
        private Tracker _tracker = new Tracker
        {
            Interval = Properties.Settings.Default.TrackerInterval,
            Enabled = false,
            ScenarioId = null,
            TrainingThreshold = Properties.Settings.Default.TrackerTrainingThreshold
        };

        /// <summary>
        /// The localization module.
        /// </summary>
        private Localizer _localizer = new Localizer
        {
            ContextId = null,
            CreationAllowed = false,
            Enabled = false,
            Interval = Properties.Settings.Default.LocalizerInterval,
            Retries = Properties.Settings.Default.LocalizerRetries,
            SimilarityAlgorithm = SimilarityAlgorithms.PredefinedAlgorithm(
                (SimilarityAlgorithmCode)Properties.Settings.Default.LocalizerSimilarityAlgorithm)
        };
        #endregion

        #region Properties

        /// <summary>
        /// The service state.
        /// </summary>
        public ServiceStateCode ServiceState { get; private set; }

        public uint NotificationThreshold { get; private set; }

        private uint ConsecutiveCorrectPredictions { get; set; }

        #endregion

        /// <summary>
        /// Service initialization.
        /// </summary>
        public LocalizationService()
        {
            // initialize properties
            ConsecutiveCorrectPredictions = 0;
            NotificationThreshold = Properties.Settings.Default.NotificationThreshold;
            ServiceState = ServiceStateCode.Idle;

            // enable handlers
            _tracker.TrackerNotification += tracker_TrackerNotification;
            _localizer.LocalizerNotification += localizer_LocalizerNotification;
        }
        


        /// <summary>
        /// Handler for notifications by the localizer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void localizer_LocalizerNotification(object sender, LocalizerNotificationEventArgs e)
        {
            // requires a lock since it's running on a different thread
            lock(_lockStatus)
            {
                // suspend prediction timer
                bool previousLocalizerStatus = _localizer.Enabled;
                _localizer.Enabled = false;

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

                        Debug.WriteLine("PREDICTION: " + e.PredictionValue.PredictedScenario.ToString() +
                            " success: " + e.PredictionValue.Success.ToString());
                        // dispatch prediction
                        if(ServiceState == ServiceStateCode.Tracking)
                        {
                            _tracker.Update();
                        }
                        DispatchPrediction(e.PredictionValue);
                        break;
                }

                // restore localizer status
                _localizer.Enabled = previousLocalizerStatus;
            }
        }

        /// <summary>
        /// Handler for notifications by the tracker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tracker_TrackerNotification(object sender, TrackerNotificationEventArgs e)
        {
            lock(_lockStatus)
            {
                switch (e.Code)
                {
                    case TrackerNotificationCode.NoSignalsDetected:
                        Debug.WriteLine("NO SIGNAL DETECTED.");
                        break;

                    case TrackerNotificationCode.Started:
                        
                        // from tracking to training mode
                        Debug.WriteLine("TRACKER STARTED (training). ");
                        _localizer.Enabled = false;

                        // notify all subscribers
                        foreach (var subscriber in subscribers)
                        {
                            if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                            {
                                var serviceStatus = new ServiceStatus
                                {
                                    ContextId = _localizer.ContextId,
                                    ServiceState = ServiceStateCode.Training
                                };
                                subscriber.ReportServiceStatus(serviceStatus);
                            }
                            else
                            {
                                subscribers.Remove(subscriber);
                            }
                        }
                        break;

                    case TrackerNotificationCode.Stopped:

                        // from tracking to notification mode
                        Debug.WriteLine("TRACKER STOPPED.");

                        // notify all subscribers
                        foreach (var subscriber in subscribers)
                        {
                            if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                            {
                                var serviceStatus = new ServiceStatus
                                {
                                    ContextId = _localizer.ContextId,
                                    ServiceState = ServiceStateCode.Notification
                                };
                                subscriber.ReportServiceStatus(serviceStatus);
                            }
                            else
                            {
                                subscribers.Remove(subscriber);
                            }
                        } 
                        break;

                    case TrackerNotificationCode.TrainingCompleted:

                        // from training to tracking mode
                        Debug.WriteLine("TRAINING COMPLETED.");
                        _localizer.Enabled = true;

                        // notify all subscribers
                        foreach (var subscriber in subscribers)
                        {
                            if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                            {
                                var serviceStatus = new ServiceStatus
                                {
                                    ContextId = _localizer.ContextId,
                                    ServiceState = ServiceStateCode.Tracking
                                };
                                subscriber.ReportServiceStatus(serviceStatus);
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
            lock(_lockStatus)
            {
                return new ServiceStatus
                {
                    ContextId = _localizer.ContextId,
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
                    TrainingThreshold = Properties.Settings.Default.TrackerTrainingThreshold
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
            lock(_lockStatus)
            {
                try
                {
                    // apply settings
                    _tracker.Interval = newSettings.Tracker.Interval;
                    _tracker.TrainingThreshold = newSettings.Tracker.TrainingThreshold;
                    _localizer.Interval = newSettings.Localizer.Interval;
                    _localizer.SimilarityAlgorithm = SimilarityAlgorithms.PredefinedAlgorithm(newSettings.Localizer.SimilarityAlgorithm);
                    _localizer.Retries = newSettings.Localizer.Retries;
                    NotificationThreshold = newSettings.NotificationThreshold;
                }
                catch
                {
                    // error applying settings
                    return false;
                }
                // save new settings
                Properties.Settings.Default.TrackerInterval = newSettings.Tracker.Interval;
                Properties.Settings.Default.TrackerTrainingThreshold = newSettings.Tracker.TrainingThreshold;
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

            lock(_lockStatus)
            {

                var previousState = ServiceState;

                try
                {
                    if(contextId == _localizer.ContextId && previousState == ServiceStateCode.Tracking)
                    {
                        // do nothing
                        return true;
                    }

                    // prepare for tracking
                    _tracker.ScenarioId = null;
                    _localizer.Enabled = false;
                    ServiceState = ServiceStateCode.Tracking;

                    // notify all subscribers
                    foreach (var subscriber in subscribers)
                    {
                        if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                        {
                            var serviceStatus = new ServiceStatus
                            {
                                ContextId = contextId,
                                ServiceState = ServiceStateCode.Tracking
                            };
                            subscriber.ReportServiceStatus(serviceStatus);
                        }
                        else
                        {
                            subscribers.Remove(subscriber);
                        }
                    }

                    if(contextId != _localizer.ContextId)
                    {
                        // prepare for new context
                        ConsecutiveCorrectPredictions = 0;
                        _localizer.ContextId = contextId;
                    }

                    // perform a first prediction
                    _localizer.CreationAllowed = true;
                    var prediction = _localizer.Prediction();
                    if(prediction == null || prediction.PredictedScenario == null)
                    {
                        return false;
                    }
                    DispatchPrediction(prediction);
                    // enable localizer timer for the next predictions
                    _localizer.Enabled = true;
                    
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
                Debug.WriteLine("DISPATCH: null prediction");
                // do nothing
                return;
            }

            lock(_lockStatus)
            {
                if(ServiceState == ServiceStateCode.Idle || ServiceState == ServiceStateCode.Training)
                {
                    Debug.WriteLine("DISPATCH: skip state " + ServiceState.ToString());
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
                    Debug.WriteLine("DISPATCH: prediction threshold passed");

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
                        Debug.WriteLine("DISPATCH: success in tracking state.");
                        // no need to track
                        _tracker.Enabled = false;
                    }
                    else
                    {
                        Debug.WriteLine("DISPATCH: failure in tracking state.");
                        // training needed
                        _tracker.ScenarioId = 
                            prediction.PredictedScenario == null
                            ? null : prediction.PredictedScenario.Id.ToString();
                        _tracker.Enabled = true;
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
            _tracker.Enabled = false;
            lock(_lockStatus)
            {
                _localizer.CreationAllowed = false;
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
            _localizer.GetScenarioForCurrentPosition(out preferences);
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
            _tracker.Enabled = false;

            lock(_lockStatus)
            {
                Debug.WriteLine("SILENCE");
                _localizer.Enabled = false;
                ServiceState = ServiceStateCode.Idle;
                // notify all subscribers
                foreach (var subscriber in subscribers)
                {
                    if (((ICommunicationObject)subscriber).State == CommunicationState.Opened)
                    {
                        var serviceStatus = new ServiceStatus
                        {
                            ContextId = _localizer.ContextId,
                            ServiceState = ServiceStateCode.Idle
                        };
                        subscriber.ReportServiceStatus(serviceStatus);
                    }
                    else
                    {
                        subscribers.Remove(subscriber);
                    }
                }
            }
        }
    }
}
