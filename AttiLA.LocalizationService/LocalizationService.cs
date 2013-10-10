using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;

namespace AttiLA.LocalizationService
{
    public class LocalizationService : ILocalizationService
    {
        #region Locks
        private System.Object lockTracking = new System.Object();
        #endregion

        #region Events
        /// <summary>
        /// Represent a method that will handle <see cref="TrackerStartNotification"/>
        /// </summary>
        /// <param name="context">The context to be tracked.</param>
        public delegate void TrackerStartNotificationHandler(Context context);

        /// <summary>
        /// Represent a method that will handle <see cref="TrackerStartNotification"/>
        /// </summary>
        /// <param name="previousContext">The context that was tracked.</param>
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
            // TODO..
        }

        public void TrackModeStart()
        {
            lock (lockTracking)
            {
                // TODO..

            }
        }

        public void TrackModeStop()
        {
            lock (lockTracking)
            {
                // TODO..

            }
        }
        
    }
}
