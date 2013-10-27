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
using System.Threading;
using System.Diagnostics;

namespace AttiLA.LocalizationService
{
    
    #region Notification classes
    /// <summary>
    /// The tracker notification codes.
    /// </summary>
    public enum TrackerNotificationCode
    {
        Started,
        Stopped,
        TrainingCompleted,
        NoSignalsDetected
    }

    /// <summary>
    /// The tracker notification error codes.
    /// </summary>
    public enum TrackerErrorNotificationCode
    {
        DatabaseError
    }

    /// <summary>
    /// Arguments of tracker notifications.
    /// </summary>
    public class TrackerNotificationEventArgs : EventArgs
    {
        /// <summary>
        /// A code to identify the notification type.
        /// </summary>
        public TrackerNotificationCode Code { get; set; }

        /// <summary>
        /// The scenario used by the tracker.
        /// </summary>
        public Scenario TargetScenario { get; set; }
    }

    /// <summary>
    /// Data for tracker error notification event handler.
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
        /// A code to identify the tracker error notification type.
        /// </summary>
        public TrackerErrorNotificationCode Code { get; set; }

        /// <summary>
        /// The exception that raised this tracker error.
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
        /// Tracker error notification event.
        /// </summary>
        public event TrackerErrorNotificationEventHandler TrackerErrorNotification;
        #endregion

        #region Private members

        /// <summary>
        /// The lock used to synchronize access to the tracker.
        /// </summary>
        private Object _trackerLock = new Object();

        /// <summary>
        /// Samples supplier module.
        /// </summary>
        private WlanScanner _wlanScanner = WlanScanner.Instance;

        /// <summary>
        /// Service to interact with scenarios in database.
        /// </summary>
        private ScenarioService _scenarioService = new ScenarioService();

        /// <summary>
        /// The timer used to capture WLAN signal samples at a certain rate.
        /// </summary>
        private System.Timers.Timer _trackerTimer = new System.Timers.Timer();

        /// <summary>
        /// A copy of the scenario on database used to save informations between updates.
        /// </summary>
        private Scenario _targetScenario;

        private uint _trainingThreshold;

        #endregion

        #region Properties

        /// <summary>
        /// The target scenario ID.
        /// </summary>
        public string ScenarioId
        {
            get
            {
                lock (_trackerLock)
                {
                    return (_targetScenario == null ? null : _targetScenario.Id.ToString());
                }
            }
            set
            {
                lock (_trackerLock)
                {
                    if (value == null)
                    {
                        _targetScenario = null;
                    }
                    else if (_targetScenario == null || !_targetScenario.Id.ToString().Equals(value))
                    {
                        if(!ScenarioService.IsValidObjectID(value))
                        {
                            throw new ArgumentOutOfRangeException("value");
                        }
                        // get scenario from database
                        _targetScenario = _scenarioService.GetById(value);
                        if (_targetScenario == null)
                        {
                            throw new ArgumentOutOfRangeException("value", Properties.Resources.MsgErrorInvalidScenarioId);
                        }

                        // use this object as container for new sampling data
                        _targetScenario.TrainingSet.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// The interval in milliseconds between captures.
        /// </summary>
        public double Interval
        {
            get
            {
                lock (_trackerLock)
                {
                    return _trackerTimer.Interval;
                }
            }
            set
            {
                lock (_trackerLock)
                {
                    _trackerTimer.Interval = value;
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
                lock (_trackerLock)
                {
                    return _trackerTimer.Enabled;
                }
            }
            set
            {
                Thread notificationThread;
                lock (_trackerLock)
                {
                    var beforeState = _trackerTimer.Enabled;
                    _trackerTimer.Enabled = value;

                    // fire the notification event only if the tracker changes state
                    if (beforeState != value && TrackerNotification != null)
                    {
                        var args = new TrackerNotificationEventArgs
                        {
                            Code = value ? TrackerNotificationCode.Started 
                                : TrackerNotificationCode.Stopped,
                            TargetScenario = _targetScenario
                        };
                        notificationThread = new Thread(() => TrackerNotification(this, args));
                        notificationThread.Start();
                        Debug.WriteLine("PIPPO");
                    }
                }
            }
        }

        /// <summary>
        /// The number of samples required to train a scenario.
        /// </summary>
        public uint TrainingThreshold
        {
            get
            {
                lock(_trackerLock)
                {
                    return _trainingThreshold;
                }
            }
            set
            {
                lock(_trackerLock)
                {
                    _trainingThreshold = value;
                }
            }
        }

        #endregion


        /// <summary>
        /// Initialize a new instance of the class <see cref="Tracker"/>
        /// </summary>
        public Tracker()
        {
            _trackerTimer.Elapsed += trackerTimer_Elapsed;
        }

        /// <summary>
        /// All the examples in the staging area are stored in the database.
        /// </summary>
        public void Update()
        {
            lock (_trackerLock)
            {
                // no more examples will be added until unlock

                if (_targetScenario == null || _targetScenario.TrainingSet.Count == 0)
                {
                    // nothing to do
                    return;
                }
                // suspend timer and store staging area in the database
                suspend();
                try
                {
                    _scenarioService.AddScanExamples(
                        _targetScenario.Id.ToString(),
                        _targetScenario.TrainingSet);

                    // erase staging area if no error detected
                    _targetScenario.TrainingSet.Clear();
                }
                catch (DatabaseException ex)
                {
                    // do not erase staging area and notify an error
                    if (TrackerErrorNotification != null)
                    {
                        var args = new TrackerErrorNotificationEventArgs(
                            TrackerErrorNotificationCode.DatabaseError, ex);
                        var t = new Thread(() => TrackerErrorNotification(this, args));
                        t.Start();
                    }
                }
                finally
                {
                    // resume timer before unlock
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
        void trackerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock(_trackerLock)
            {
                if(_targetScenario == null)
                {
                    // nothing to do
                    return;
                }

                // suspend capture until completion
                _trackerTimer.Stop();

                var scanSignals = _wlanScanner.GetScanSignals();
                if(scanSignals.Count == 0)
                {
                    // notify and ignore signals
                    if (TrackerNotification != null)
                    {
                        var args = new TrackerNotificationEventArgs
                        {
                            Code = TrackerNotificationCode.NoSignalsDetected
                        };
                        var t = new Thread(() => TrackerNotification(this, args));
                        t.Start();
                    }
                }
                else
                {
                    var example = new TrainingSetExample
                    {
                        ScanDateTime = DateTime.Now,
                        ScanSignals = scanSignals
                    };

                    _targetScenario.TrainingSet.Add(example);

                    if(_targetScenario.TrainingSet.Count == TrainingThreshold)
                    {
                        if(TrackerNotification != null)
                        {
                            var args = new TrackerNotificationEventArgs
                            {
                                Code = TrackerNotificationCode.TrainingCompleted,
                                TargetScenario = _targetScenario
                            };
                            var t = new Thread(() => TrackerNotification(this, args));
                            t.Start();
                        }
                    }
                }

                // resume capture before unlock
                _trackerTimer.Start();
            }
        }

        /// <summary>
        /// Resume timers
        /// </summary>
        private void resume()
        {
            _trackerTimer.Start();
        }

        /// <summary>
        /// Suspend timers
        /// </summary>
        private void suspend()
        {
            _trackerTimer.Stop();
        }
        

    }
}
