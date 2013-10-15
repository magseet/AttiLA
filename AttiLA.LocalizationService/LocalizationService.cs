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
        /// <summary>
        /// Represents a method that will handle <see cref="TrackerStartNotification"/>
        /// </summary>
        /// <param name="scenario">The scenario to be tracked.</param>
        public delegate void TrackerStartNotificationHandler(Context context);

        /// <summary>
        /// Represents a method that will handle <see cref="TrackerStartNotification"/>
        /// </summary>
        /// <param name="previousContext">The scenario that was tracked.</param>
        public delegate void TrackerStopNotificationHandler(Context previousContext);

        /// <summary>
        /// Occurs when there is a request to enter in track mode.
        /// </summary>
        public event TrackerStartNotificationHandler TrackerStartNotification;

        /// <summary>
        /// Occurs when there is a request to leave the track mode.
        /// </summary>
        public event TrackerStopNotificationHandler TrackerStopNotification;

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
            //SimilarityAlgorithm = new SimilarityAlgorithm()[Properties.Settings.Default.]
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
        /// Record the subscribers to the callback service.
        /// </summary>
        private static readonly List<ILocalizationServiceCallback> 
            subscribers = new List<ILocalizationServiceCallback>();


        public LocalizationService()
        {
            tracker.TrackerNotification += tracker_TrackerNotification;
        }

        /// <summary>
        /// Handler for notifications from tracker.
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
                    subscribers.ForEach(delegate(ILocalizationServiceCallback callback)
                    {
                        if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                        {
                            callback.TrackModeStarted(startTime);
                        }
                        else
                        {
                            subscribers.Remove(callback);
                        }
                    });
                    break;

                case TrackerNotificationCode.Stop:
                    // notify all subscribers about tracker stop
                    var stopTime = DateTime.Now;
                    subscribers.ForEach(delegate(ILocalizationServiceCallback callback)
                    {
                        if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                        {
                            callback.TrackModeStopped(stopTime);
                        }
                        else
                        {
                            subscribers.Remove(callback);
                        }
                    });
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

        public string Localize(bool changeContext, out IEnumerable<ContextSimilarity> similarContexts)
        {
            Scenario scenario = localizer.Prediction(out similarContexts);
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
