using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BleDA.LocalizationService;
using System.ServiceModel;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using System.Threading;

namespace BleDA
{
    #region Notification classes

    public enum UserInteractionCode
    {
        /// <summary>
        /// The localization service is running on a context.
        /// </summary>
        PreviousContextFound,
        /// <summary>
        /// The selected context is different than previous one.
        /// </summary>
        NewContextSelected,
        /// <summary>
        /// The service has been silenced due to difficulties in localizing.
        /// </summary>
        TrackingSessionFailed,
        /// <summary>
        /// The service has correctly terminated a tracking session.
        /// </summary>
        TrackingSessionSucceded
    }

    /// <summary>
    /// Arguments of localizer notifications.
    /// </summary>
    public class UserInteractionEventArgs : EventArgs
    {
        private object value;

        /// <summary>
        /// Value casting for previous context found notification event.
        /// </summary>
        public string PreviousContextFoundValue
        {
            get
            {
                return (string)value;
            }

            set
            {
                this.value = (string)value;
            }
        }


        /// <summary>
        /// A code to identify the notification type.
        /// </summary>
        public UserInteractionCode Code { get; set; }

    }

    /// <summary>
    /// The status notification error codes.
    /// </summary>
    public enum StatusErrorNotificationCode
    {
        /// <summary>
        /// The service responded with an unexpected context id.
        /// </summary>
        UnexpectedPrediction
    }


    /// <summary>
    /// Data for status error notification event handler.
    /// </summary>
    public class StatusErrorNotificationEventArgs : EventArgs
    {
        public StatusErrorNotificationEventArgs(StatusErrorNotificationCode code)
        {
            Code = code;
        }

        public StatusErrorNotificationEventArgs(StatusErrorNotificationCode code, Exception cause)
        {
            Code = code;
            Cause = cause;
        }

        /// <summary>
        /// A code to identify the status error notification type.
        /// </summary>
        public StatusErrorNotificationCode Code { get; set; }

        /// <summary>
        /// The exception that raised this status error.
        /// </summary>
        public Exception Cause { get; set; }
    }

    #endregion

    #region Exceptions

    public enum StatusExceptionCode
    {
        /// <summary>
        /// The service did not respond as expected
        /// </summary>
        ServiceFailure
    }

    /// <summary>
    /// Information about a system operation failure.
    /// </summary>
    public class StatusException : Exception
    {
        /// <summary>
        /// The service exception code.
        /// </summary>
        public StatusExceptionCode Code { get; set; }

        public StatusException() : base() { }
        public StatusException(string message) : base(message) { }
        public StatusException(string message, System.Exception inner) : base(message, inner) { }
    }

    #endregion

    public sealed class Status : ILocalizationServiceCallback
    {
        private static volatile Status _instance;
        private static object lockStatus = new Object();
        private Process _process;
        private LocalizationServiceClient _serviceClient;
        private string _currentContextId;
        private System.Timers.Timer _timer = new System.Timers
            .Timer(Properties.Settings.Default.ClientTimeout);

        #region Events
        public delegate void UserInteractionEventHandler(object sender, EventArgs e);
        public event UserInteractionEventHandler UserInteraction;
        #endregion

        private Status()
        {
            CurrentContextId = "";

            var context = new InstanceContext(this);
            _serviceClient = new LocalizationServiceClient(context);
            _serviceClient.Subscribe();
            _timer.Elapsed += _timer_Elapsed;

        }

        /// <summary>
        /// Localization service status
        /// </summary>
        public ServiceStatus ServiceStatus
        {
            get
            {
                lock (lockStatus)
                {
                    if (_serviceClient == null)
                    {
                        return null;
                    }
                    return _serviceClient.GetServiceStatus();
                }
            }
        }

        /// <summary>
        /// Timer event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();

            State nextState = State.Unknown, leavingState = _process.CurrentState;
            UserInteractionEventArgs args;
            Thread notificationThread;

            lock (lockStatus)
            {
                switch (leavingState)
                {
                    case State.WaitForCorrectPrediction:
                        args = new UserInteractionEventArgs
                        {
                            Code = UserInteractionCode.TrackingSessionFailed
                        };
                        notificationThread = new Thread(() => UserInteraction(this, args));
                        notificationThread.Start();

                        nextState = _process.MoveNext(Command.None);
                        break;

                    case State.Tracking:
                        args = new UserInteractionEventArgs
                        {
                            Code = UserInteractionCode.TrackingSessionSucceded
                        };
                        notificationThread = new Thread(() => UserInteraction(this, args));
                        notificationThread.Start();

                        nextState = _process.MoveNext(Command.None);
                        break;

                    default:
                        break;
                }

                EnterState(nextState);
            }
        }

        public static Status Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockStatus)
                    {
                        if (_instance == null)
                            _instance = new Status();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// User selected context.
        /// </summary>
        public String CurrentContextId
        {
            get
            {
                lock (lockStatus)
                {
                    return _currentContextId;
                }
            }
            set
            {
                lock (lockStatus)
                {
                    _currentContextId = value;
                }
            }
        }

        

        /// <summary>
        /// Perform status initialization.
        /// </summary>
        public void Initialize()
        {
            lock (lockStatus)
            {
                if (_process != null)
                {
                    // already initialized
                    return;
                }
                _process = new Process();
                EnterState(_process.CurrentState); // state machine reset

                // determine first operational state
                State nextState;
                if (ServiceStatus.ContextId == null)
                {
                    nextState = _process.MoveNext(Command.Selection);
                }
                else
                {
                    nextState = _process.MoveNext(Command.Confirmation);
                }
                EnterState(nextState);
            }
        }

        /// <summary>
        /// Inform about User context selection
        /// </summary>
        /// <param name="contextId"></param>
        /// <exception cref="SettingsException"></exception>
        /// <returns></returns>
        public bool ContextSelected(string contextId)
        {
            if (contextId == null)
            {
                throw new ArgumentNullException("contextId");
            }

            if (!EntityService<Context>.IsValidObjectID(contextId))
            {
                throw new ArgumentOutOfRangeException("contextId");
            }
            lock (lockStatus)
            {

                State nextState;
                if (contextId != CurrentContextId)
                {
                    // notify that AttiLA has a different context
                    var args = new UserInteractionEventArgs
                    {
                        Code = UserInteractionCode.NewContextSelected
                    };
                    var t = new Thread(() => UserInteraction(this, args));
                    t.Start();

                    CurrentContextId = contextId;
                    // new context selected
                    nextState = _process.MoveNext(Command.Selection);
                }
                else
                {
                    // confirmation
                    nextState = _process.MoveNext(Command.Confirmation);
                }

                EnterState(nextState);
            }
            return true;
        }

        private void EnterState(State state)
        {
            lock (lockStatus)
            {
                switch (_process.CurrentState)
                {
                    case State.Idle:
                        // retrieve service status
                        ServiceStatus serviceStatus = null;
                        for (var attempts = Properties.Settings.Default.ClientRetries + 1;
                            attempts > 0; attempts--)
                        {
                            try
                            {
                                serviceStatus = ServiceStatus;
                                if (serviceStatus != null)
                                    break;
                            }
                            catch { }

                        }

                        if (serviceStatus != null && serviceStatus.ContextId != null)
                        {
                            // notify that AttiLA had a context
                            var args = new UserInteractionEventArgs
                            {
                                Code = UserInteractionCode.PreviousContextFound,
                                PreviousContextFoundValue = serviceStatus.ContextId
                            };
                            var t = new Thread(() => UserInteraction(this, args));
                            t.Start();
                        }

                        break;

                    case State.WaitForSelection:
                        //Eventually stop notifications
                        _serviceClient.Silence();
                        break;

                    case State.WaitForCorrectPrediction:
                        _timer.Enabled = true;
                        // tracking start
                        bool trackingStarted = false;
                        for (var attempts = Properties.Settings.Default.ClientRetries + 1;
                            attempts > 0; attempts--)
                        {
                            try
                            {
                                trackingStarted = _serviceClient.TrackingStart(CurrentContextId);
                                if (trackingStarted)
                                    break;
                            }
                            catch { }
                        }
                        if (!trackingStarted)
                        {
                            throw new SettingsException(Properties.Resources.MsgTrackingStartFailure)
                            {
                                Code = SettingsExceptionCode.ServiceFailure,
                            };
                        }
                        break;

                    case State.WaitForWrongPrediction:
                        //ToDO: 
                        break;
                    case State.WaitForConfirmation:
                        //ToDO: 
                        break;
                    case State.Tracking:
                        //ToDO: 
                        break;
                    default:
                        break;
                }
            }
        }


        #region ILocalizationServiceCallback methods

        public void ReportLocalizationProgress(double progress)
        {
            // update status

        }

        public void ReportPrediction(string contextId)
        {

            lock (lockStatus)
            {
                if (_process.CurrentState == State.WaitForCorrectPrediction)
                {
                    if (contextId == null || contextId != _currentContextId)
                    {
                        // error notification
                        var args = new StatusErrorNotificationEventArgs(
                            StatusErrorNotificationCode.UnexpectedPrediction);
                        var t = new Thread(() => UserInteraction(this, args));
                        t.Start();
                    }
                    else
                    {
                        // expected prediction
                        var nextState = _process.MoveNext(Command.CorrectPrediction);
                        EnterState(nextState);
                    }
                }
                else if (_process.CurrentState == State.WaitForWrongPrediction)
                {
                    if (contextId == null || contextId == _currentContextId)
                    {
                        // error notification
                        var args = new StatusErrorNotificationEventArgs(
                            StatusErrorNotificationCode.UnexpectedPrediction);
                        var t = new Thread(() => UserInteraction(this, args));
                        t.Start();
                    }
                    else
                    {
                        // expected prediction
                        var nextState = _process.MoveNext(Command.WrongPrediction);
                        EnterState(nextState);
                    }

                }
            }
        }

        public void ReportServiceStatus(ServiceStatus serviceStatus)
        {
            //

        }

        #endregion

    }
}
