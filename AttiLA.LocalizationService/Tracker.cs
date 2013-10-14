using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;
using AttiLA.Data;
using System.Timers;
using MongoDB.Bson;

namespace AttiLA.LocalizationService
{
    
    #region Notification classes
    /// <summary>
    /// The notification codes.
    /// </summary>
    public enum TrackerNotificationCode
    {
        Start,
        Stop,
        Update,
        NoSignalsDetected
    }

    /// <summary>
    /// The notification deltaSimilarity codes.
    /// </summary>
    public enum TrackerErrorNotificationCode
    {
        DatabaseError
    }

    /// <summary>
    /// Data for to notification event handler.
    /// </summary>
    public class TrackerNotificationEventArgs : EventArgs
    {
        public TrackerNotificationEventArgs(TrackerNotificationCode code)
        {
            Code = code;
        }

        /// <summary>
        /// A code to identify the notification type.
        /// </summary>
        public TrackerNotificationCode Code { get; private set; }

    }

    /// <summary>
    /// Data for deltaSimilarity notification event handler.
    /// </summary>
    public class TrackerErrorNotificationEventArgs : EventArgs
    {
        public TrackerErrorNotificationEventArgs(TrackerErrorNotificationCode code)
        {
            Code = code;
        }

        public TrackerErrorNotificationEventArgs(TrackerErrorNotificationCode code, Exception cause)
        {
            Code = code;
            Cause = cause;
        }

        /// <summary>
        /// A code to identify the deltaSimilarity notification type.
        /// </summary>
        public TrackerErrorNotificationCode Code { get; set; }

        /// <summary>
        /// The exception that raised this deltaSimilarity.
        /// </summary>
        public Exception Cause { get; set; }
    }
    #endregion


    /// <summary>
    /// The tracker samples periodically the WLAN signals and updates the scenario
    /// in the database.
    /// This class is thread safe.
    /// </summary>
    public class Tracker
    {
        #region Events
        /// <summary>
        /// Represents a method that will handle <see cref="TrackerNotificationEvent"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TrackerNotificationEventHandler(object sender, TrackerNotificationEventArgs e);

        /// <summary>
        /// Represents a method that will handle <see cref="TrackerErrorNotificationEvent"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TrackerErrorNotificationEventHandler(object sender, TrackerErrorNotificationEventArgs e);

        /// <summary>
        /// Tracker notification event.
        /// </summary>
        public event TrackerNotificationEventHandler TrackerNotification;

        /// <summary>
        /// Tracker deltaSimilarity notification event.
        /// </summary>
        public event TrackerErrorNotificationEventHandler TrackerErrorNotification;
        #endregion

        /// <summary>
        /// Samples supplier module.
        /// </summary>
        private WlanScanner wlanScanner = WlanScanner.Instance;

        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private ScenarioService scenarioService = new ScenarioService();

        /// <summary>
        /// The timer used to capture WLAN signal samples at a certain rate.
        /// </summary>
        private Timer captureTimer = new Timer();

        /// <summary>
        /// The timer used to save all the captured samples in the scenario on the database.
        /// </summary>
        private Timer updateTimer = new Timer();

        /// <summary>
        /// The lock used to synchronize access to the tracker.
        /// </summary>
        private Object trackerLock = new Object();

        /// <summary>
        /// A copy of the scenario on database used to save informations between updates.
        /// </summary>
        private Scenario targetScenario;

        /// <summary>
        /// The target scenario ID.
        /// </summary>
        public string ScenarioId
        {
            get
            {
                lock (trackerLock)
                {
                    if (targetScenario == null)
                    {
                        return null;
                    }
                    return targetScenario.Id.ToString();
                    
                }
            }
            set
            {
                lock (trackerLock)
                {
                    if(value == null)
                    {
                        targetScenario = null;
                    }
                    else if(targetScenario == null || !targetScenario.Id.ToString().Equals(value))
                    {
                        // get scenario from database
                        targetScenario = scenarioService.GetById(value);
                        if(targetScenario == null)
                        {
                            throw new ArgumentOutOfRangeException("value", Properties.Resources.MsgErrorInvalidScenarioId);
                        }

                        // use this object as container for new sampling data
                        targetScenario.TrainingSet.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// The interval in milliseconds between captures.
        /// </summary>
        public double CaptureInterval
        {
            get
            {
                lock (trackerLock)
                {
                    return captureTimer.Interval;
                }
            }
            set
            {
                lock(trackerLock)
                {
                    captureTimer.Interval = value;
                }
            }
        }

        /// <summary>
        /// The interval in milliseconds between updates.
        /// </summary>
        public double UpdateInterval
        {
            get
            {
                lock(trackerLock)
                { 
                    return updateTimer.Interval;
                }
            }
            set
            {
                lock(trackerLock)
                {
                    updateTimer.Interval = value;
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
                lock (trackerLock)
                {
                    return captureTimer.Enabled;
                }
            }
            set
            {
                lock(trackerLock)
                {
                    captureTimer.Enabled = value;
                    updateTimer.Enabled = value;
                }
            }
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="Tracker"/>
        /// </summary>
        public Tracker()
        {
            captureTimer.Elapsed += captureTimer_Elapsed;
            updateTimer.Elapsed += updateTimer_Elapsed;
        }

        /// <summary>
        /// Each time this handler is invoked, all the examples in the staging
        /// area are stored in the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock(trackerLock)
            {
                // no more examples will be added until unlock

                if(targetScenario == null || targetScenario.TrainingSet.Count == 0)
                {
                    // nothing to do
                    return;
                }
                // suspend timers and store staging area in the database
                suspend();
                try
                {
                    scenarioService.AddScanExamples(
                        targetScenario.Id.ToString(),
                        targetScenario.TrainingSet);

                    // erase staging area if no deltaSimilarity detected
                    targetScenario.TrainingSet.Clear();
                }
                catch(DatabaseException ex)
                {
                    // do not erase staging area and notify an deltaSimilarity
                    if(TrackerErrorNotification != null)
                    {
                        TrackerErrorNotification(this, new TrackerErrorNotificationEventArgs(
                            TrackerErrorNotificationCode.DatabaseError, ex));
                    }
                }
                finally
                {
                    // resume timers before unlock
                    resume();
                }

            }
            
        }

        /// <summary>
        /// Each time this handler is invoked, a new WLAN scan is performed
        /// and a new example is added to the staging area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void captureTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock(trackerLock)
            {
                if(targetScenario == null)
                {
                    // nothing to do
                    return;
                }

                // suspend capture until completion
                captureTimer.Stop();

                var scanSignals = wlanScanner.GetScanSignals();
                if(scanSignals.Count == 0)
                {
                    // notify and ignore signals
                    if (TrackerNotification != null)
                    {
                        TrackerNotification(this, new TrackerNotificationEventArgs(
                            TrackerNotificationCode.NoSignalsDetected));
                    }
                }
                else
                {
                    targetScenario.TrainingSet.Add(new TrainingSetExample
                    {
                        ScanDateTime = DateTime.Now,
                        ScanSignals = scanSignals
                    });
                }

                // resume capture before unlock
                captureTimer.Start();
            }
        }

        /// <summary>
        /// Resume timers
        /// </summary>
        private void resume()
        {
            captureTimer.Start();
            updateTimer.Start();
        }

        /// <summary>
        /// Suspend timers
        /// </summary>
        private void suspend()
        {
            captureTimer.Stop();
            updateTimer.Stop();
        }
        


    }
}
