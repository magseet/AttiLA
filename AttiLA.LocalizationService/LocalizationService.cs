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
        private System.Object lockTracking = new System.Object();
        #endregion

        #region Events


        #endregion

        /// <summary>
        /// The tracking module.
        /// </summary>
        private Tracker tracker = new Tracker 
        { 
            CaptureInterval = Properties.Settings.Default.TrackerCaptureInterval,
            UpdateInterval = Properties.Settings.Default.TrackerUpdateInterval,
            Enabled = Properties.Settings.Default.TrackerEnabledOnStartup
        };

        /// <summary>
        /// The localization module.
        /// </summary>
        private Localizer localizer = new Localizer
        {
            Retries = Properties.Settings.Default.LocalizerRetries,
            SimilarityAlgorithm = SimilarityAlgorithms.PredefinedAlgorithm(SimilarityAlgorithmType.RelativeError)
        };

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


        public LocalizationService()
        {
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

        /// <summary>
        /// Handler for notifications by the tracker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tracker_TrackerNotification(object sender, TrackerNotificationEventArgs e)
        {
            switch(e.Code)
            {
                case TrackerNotificationCode.NoSignalsDetected:
                    break;

                case TrackerNotificationCode.Start:
                    // notify all subscribers about tracker start
                    var startTime = DateTime.Now;
                    foreach(var subscriber in subscribers)
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
                    foreach(var subscriber in subscribers)
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

        public GlobalSettings GetGlobalSettings()
        {
            // TODO..
            return null;
        }

        public void SetGlobalSettings(GlobalSettings newSettings)
        {
            // TODO..
        }

        public void ChangeContext(string newContextId)
        {

            if(newContextId == null || !IsValidObjectId(newContextId))
            {
                throw new FaultException<ArgumentException>(
                    new ArgumentException("newContextId"));
            }

            // TODO.. localize and check if prediction was right

            // dummy case
            var context = contextService.GetById(newContextId);
            if(context == null)
            {
                throw new FaultException<ServiceException>(
                    new ServiceException
                    {
                        Message = Properties.Resources.MsgFaultContextNotFound
                    });
            }

            var scenario = new Scenario
            {
                CreationTime = DateTime.Now,
                ContextId = context.Id
            };

            try
            {
                scenarioService.Create(scenario);
            }
            catch(DatabaseException)
            {
                throw new FaultException<ServiceException>(
                    new ServiceException
                    {
                        Message = Properties.Resources.MsgFaultDatabaseError
                    });
            }

            tracker.ScenarioId = scenario.Id.ToString();

            
        }

        public void TrackModeStart()
        {
            tracker.Enabled = true;
        }

        public void TrackModeStop()
        {
            tracker.Enabled = false;
        }

        public string Localize(bool changeContext, out IEnumerable<ContextPreference> preferences)
        {
            Scenario scenario = localizer.Prediction(out preferences);
            if(changeContext)
            {
                tracker.ScenarioId = (scenario == null ? null : scenario.Id.ToString());
            }

            return scenario == null ? null : scenario.Id.ToString();
        }


        private static bool IsValidObjectId(string id)
        {
            return Regex.Match(id, "^[0-9a-fA-F]{24}$").Success;
        }
    }
}
