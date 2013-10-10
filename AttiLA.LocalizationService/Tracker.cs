using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;
using System.Timers;

namespace AttiLA.LocalizationService
{
    public class Tracker
    {
        /// <summary>
        /// The timer used to sample WLAN signal at a certain rate (AutoReset is enabled).
        /// </summary>
        private Timer samplingTimer = new Timer();

        /// <summary>
        /// The lock used to synchronize access to the tracker.
        /// </summary>
        private Object trackerLock = new Object();

        /// <summary>
        /// A copy used to save samples until context update.
        /// </summary>
        private Context targetContext;


        /// <summary>
        /// Property used to enable/disable the tracker.
        /// The access to this property is thread safe.
        /// </summary>
        public bool Enabled
        {
            get
            {
                lock (trackerLock)
                {
                    return samplingTimer.Enabled;
                }
            }
            set
            {
                lock(trackerLock)
                {
                    if (value)
                    {
                        samplingTimer.Interval = Properties.Settings.Default.TrackerInterval;
                    }
                    samplingTimer.Enabled = value;
                }
            }
        }

        


    }
}
