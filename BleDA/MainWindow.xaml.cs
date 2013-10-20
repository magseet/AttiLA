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
        public MainWindow()
        {
            InitializeComponent();

            //Modificare
            Thread StateMachine = new Thread(Start);
            StateMachine.Start();
        }

        private void Start()
        {
            var context = new InstanceContext(this);
            LocalizationServiceClient _serviceClient = new LocalizationServiceClient(context);
            _serviceClient.Subscribe();

            Process p = new Process();

            switch(p.CurrentState.Name){
                case State.Idle:
                    //ToDO: Load last context and CurrentState = WaitForUser
                    break;
                case State.WaitForUser:
                    //ToDO: CurrentState = WaitFirstPredication
                    break;
                case State.WaitFirstPrediction:
                    //ToDO: Start TimeOut (in), tracking start (in), check matching 
                    //      between selectede context and predicted context
                    break;
                case State.WaitNotifications:
                    //ToDO: 
                    break;
                case State.WaitForUser2:
                    //ToDO: 
                    break;
                case State.Tracking:
                    //ToDO: 
                    break;
                default:
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
