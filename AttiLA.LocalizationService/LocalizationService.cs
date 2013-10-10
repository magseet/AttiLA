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
        /// Occurs when there is a request to enter in Track mode.
        /// </summary>
        public event TrackerStartNotificationHandler TrackerStartNotification;

        /// <summary>
        /// Occurs when there is a request to leave the Track mode.
        /// </summary>
        public event TrackerStopNotificationHandler TrackerStopNotification;

        #endregion


        /// <summary>
        /// Changes the sampling rate used in Track mode.
        /// </summary>
        /// <param name="milliseconds"></param>
        public void SetTrackerInterval (double milliseconds)
        {
            Properties.Settings.Default.TrackerInterval = milliseconds;
        }


        public void TrackContext(string contextId)
        {
            if(String.IsNullOrWhiteSpace(contextId))
            {
                //// Handle fault here..
            }

            lock (lockTracking)
            {

            }

        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
