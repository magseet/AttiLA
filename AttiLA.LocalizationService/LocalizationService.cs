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
        private Tracker tracker = new Tracker();

        /// <summary>
        /// The localization module.
        /// </summary>
        private Localizer localizer = new Localizer();

        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private ScenarioService scenarioService = new ScenarioService();

        /// <summary>
        /// Service to interact with contexts in database.
        /// </summary>
        private ContextService contextService = new ContextService();


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

        private static bool IsValidObjectId(string id)
        {
            return Regex.Match(id, "^[0-9a-fA-F]{24}$").Success;
        }
    }
}
