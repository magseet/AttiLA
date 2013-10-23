using BleDA.AttiLA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for Starting.xaml
    /// </summary>
    public partial class Starting : UserControl, ISwitchable, ILocalizationServiceCallback
    {
        public Status Status;
        private object lockStatus = new object();
        private System.Timers.Timer timer;
        public LocalizationServiceClient _serviceClient;

        public Starting()
        {
            InitializeComponent();
            Status = Status.BleDAStatus;
            if (!Status.CurrentContextId.Equals(""))
            {
                var context = new InstanceContext(this);
                _serviceClient = new LocalizationServiceClient(context);
                _serviceClient.Subscribe();

                EnterState(Status.Process.CurrentState); // state machine reset
            }
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            State nextState;
            //Determino da dove sono scattato
            switch (Status.Process.CurrentState)
            {
                case State.WaitForCorrectPrediction:
                    nextState = Status.Process.MoveNext(Command.None);

                    if (nextState == State.Ignore)
                        break;
                    //Notificare che "Attila non risponde"
                    EnterState(nextState);
                    break;

                case State.Tracking:
                    nextState = Status.Process.MoveNext(Command.None);
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

        public void EnterState(State state)
        {
            lock (lockStatus)
            {
                switch (Status.Process.CurrentState)
                {
                    case State.Idle:
                        //ToDO: Load last context and CurrentState = WaitForSelection
                        Status.ServiceStatus = _serviceClient.GetServiceStatus();
                        if (Status.ServiceStatus.ContextId == null)
                        {
                            var nextState = Status.Process.MoveNext(Command.Selection);

                            if (nextState == State.Ignore)
                                break;

                            EnterState(nextState);
                        }
                        else
                        {
                            var nextState = Status.Process.MoveNext(Command.Confirmation);

                            if (nextState == State.Ignore)
                                break;

                            // Notifica "Attila è attivo"
                            EnterState(nextState);
                        }
                        break;

                    case State.WaitForSelection:
                        _serviceClient.SilenceAsync();   //Non fa notificare
                        // CRITICAL: se ritorno su questo controllo e non ho cambiato stato, va in eccezione.
                        break;

                    case State.WaitForCorrectPrediction:
                        // ToDO: Start TimeOut (in), tracking start (in), quando clicco sulla messagebox
                        // della selezione utente (tasto track), disabilito il timer e confrontro tra contextId
                        // e selectedContextId e lo salvo nello Status
                        timer.Elapsed += timer_Elapsed;
                        _serviceClient.TrackingStartAsync(Status.CurrentContextId);
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

        #region LocalizationService
        public void ReportLocalizationProgress(double progress)
        {
            throw new NotImplementedException();
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

        private void btnGoContext_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Context());
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ManagedProfile(), new { returnPage = "Starting" });
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Settings(), new { returnPage = "Starting" });
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new About(), new { returnPage = "Starting" });
        }
    }
}
