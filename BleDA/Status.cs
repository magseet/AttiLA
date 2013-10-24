using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BleDA.AttiLA;
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
        PreviousContextFound
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
    #endregion

    public sealed class Status : ILocalizationServiceCallback
    {
        private static volatile Status _instance;
        private static object lockStatus = new Object();
        private Process _process;
        private LocalizationServiceClient _serviceClient;
        private string _currentContextId;
        private ServiceStatus _serviceStatus;
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

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            State nextState;
            //Determino da dove sono scattato
            switch (_process.CurrentState)
            {
                case State.WaitForCorrectPrediction:
                    nextState = _process.MoveNext(Command.None);

                    if (nextState == State.Ignore)
                        break;
                    //Notificare che "Attila non risponde"
                    EnterState(nextState);
                    break;

                case State.Tracking:
                    nextState = _process.MoveNext(Command.None);
                    if (nextState == State.Ignore)
                        break;
                    // Ho finito la sessione di Tracking e notifico che sono sicuro di essere in quel context.
                    EnterState(nextState);
                    break;

                default:
                    // Non dovrebbe mai accadere!
                    break;
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
        /// AttiLA localization service status
        /// </summary>
        public ServiceStatus ServiceStatus
        {
            get
            {
                lock (lockStatus)
                {
                    return _serviceStatus;
                }
            }
            set
            {
                lock (lockStatus)
                {
                    _serviceStatus = value;
                }
            }
        }




        #region ILocalizationServiceCallback

        public void ReportLocalizationProgress(double progress)
        {
            // update status

        }

        public void ReportPrediction(string contextId)
        {
            throw new NotImplementedException();
        }

        public void ReportServiceStatus(ServiceStatus serviceStatus)
        {
            throw new NotImplementedException();
        }

        #endregion

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
                if (_serviceStatus.ContextId == null)
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


        public bool UserInputPerformed(string contextId)
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
                        for (var attempts = Properties.Settings.Default.ClientRetries + 1;
                            attempts > 0; attempts--)
                        {
                            try
                            {
                                _serviceStatus = _serviceClient.GetServiceStatus();
                            }
                            catch { }
                        }

                        if (_serviceStatus != null && _serviceStatus.ContextId != null)
                        {
                            // notify that AttiLA had a context
                            var args = new UserInteractionEventArgs
                            {
                                Code = UserInteractionCode.PreviousContextFound,
                                PreviousContextFoundValue = _serviceStatus.ContextId
                            };
                            var t = new Thread(() => UserInteraction(this, args));
                            t.Start();
                        }

                        break;

                    case State.WaitForSelection:
                        Status.Instance._serviceClient.SilenceAsync();   //Non fa notificare
                        // CRITICAL: se ritorno su questo controllo e non ho cambiato stato, va in eccezione.
                        break;

                    case State.WaitForCorrectPrediction:
                        // ToDO: Start TimeOut (in), tracking start (in), quando clicco sulla messagebox
                        // della selezione utente (tasto track), disabilito il timer e confrontro tra contextId
                        // e selectedContextId e lo salvo nello Status
                        timer.Elapsed += timer_Elapsed;
                        Status.Instance._serviceClient.TrackingStartAsync(Status.CurrentContextId);
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
    }
}
