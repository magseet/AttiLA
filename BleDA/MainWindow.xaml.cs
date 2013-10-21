using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using BleDA.AttiLA;
using System.ServiceModel;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILocalizationServiceCallback
    {
        Status s = new Status();
        private object lockStatus = new object();
        private System.Timers.Timer timer;
        LocalizationServiceClient _serviceClient;


        public MainWindow()
        {
            InitializeComponent();

            var context = new InstanceContext(this);
            _serviceClient = new LocalizationServiceClient(context);
            _serviceClient.Subscribe();
            EnterState(s.Process.CurrentState); // state machine reset

        }

        public void EnterState(State state)
        {
            lock (lockStatus)
            {
                switch (s.Process.CurrentState)
                {
                    case State.Idle:
                        //ToDO: Load last context and CurrentState = WaitForSelection
                        s.ServiceStatus = _serviceClient.GetServiceStatus();
                        if (s.ServiceStatus.ContextId == null)
                        {
                            var nextState = s.Process.MoveNext(Command.Selection);

                            if (nextState == null)
                                break;

                            EnterState(nextState);
                            
                        }
                        else
                        {
                            
                            var nextState = s.Process.MoveNext(Command.Confirmation);

                            if (nextState == null)
                                break;
                            
                            // Notifica "Attila è attivo"
                            EnterState(nextState);
                        }
                        break;

                    case State.WaitForSelection:
                        _serviceClient.Silence();   //Non fa notificare
                        break;

                    case State.WaitForCorrectPrediction:
                        // ToDO: Start TimeOut (in), tracking start (in), quando clicco sulla messagebox
                        // della selezione utente (tasto track), disabilito il timer e confrontro tra contextId
                        // e selectedContextId e lo salvo nello Status s
                        timer.Elapsed += timer_Elapsed;
                        _serviceClient.TrackingStartAsync(s.CurrentContextId);
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

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            State nextState;
            //Determino da dove sono scattato
            switch (s.Process.CurrentState)
            {
                case State.WaitForCorrectPrediction:
                    nextState = s.Process.MoveNext(Command.None);

                    if (nextState == null)
                                break;
                    //Notificare che "Attila non risponde"
                    EnterState(nextState);
                    break;

                case State.Tracking:
                    nextState = s.Process.MoveNext(Command.None);
                    if (nextState == null)
                                break;
                    // Ho finito la sessione di Tracking e notifico che sono sicuro di essere in quel context.
                    EnterState(nextState);
                    break;

                default:
                    // Non dovrebbe mai accadere!
                    break;
            }
        }

        public void ReportLocalizationProgress(double progress)
        {
            return;
        }

        public void ReportPrediction(string contextId)
        {
            //Transizione del processo, spia
            return;
        }

        public void ReportServiceStatus(ServiceStatus serviceStatus)
        {
            //Aggiornare stato di attila sotto lock!
            return;
        }
    }
}
