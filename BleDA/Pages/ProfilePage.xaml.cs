using System;
using System.Collections.Generic;
using System.Linq;
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
using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using BleDA.LocalizationService;
using System.ServiceModel;

namespace BleDA
{



    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    /// 
    public partial class ProfilePage : UserControl, ISwitchable, ILocalizationServiceCallback
    {
        Status _status = Status.Instance;
        ContextService _contextService = new ContextService();
        System.Timers.Timer _refreshTimer = new System.Timers.Timer();
        Settings _settings = Settings.Instance;
        private LocalizationServiceClient _serviceClient;


        public ProfilePage()
        {
            InitializeComponent();
            var context = new InstanceContext(this);
            _serviceClient = new LocalizationServiceClient(context);

            _status.StatusErrorNotification += _status_StatusErrorNotification;
            _status.UserInteraction += _status_UserInteraction;

            getContext();

            //_refreshTimer.Elapsed += _refreshTimer_Elapsed;
            //_refreshTimer.Interval = _settings.FindRefreshInterval;
            //_refreshTimer.Enabled = true;



        }

        void _refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Suspend timer
            _refreshTimer.Stop();

            var getContextCall = new System.Action(getContext);

            getContextCall.Invoke();

            //resume timer
            _refreshTimer.Start();
        }

        private void getContext()
        {
            var recentContextList = _contextService.GetMostRecent((int)_settings.MostRecentLimit);
            var contexts = _serviceClient.GetCloserContexts();
            
            List<ContextPreferenceExtended> closerContextList = new List<ContextPreferenceExtended>();
            foreach (var item in contexts)
	        {
                var context = _contextService.GetById(item.ContextId);
                if(context == null)
                    continue;

		        closerContextList.Add(new ContextPreferenceExtended 
                { ContextName = context.ContextName,
                    Value = item.Value.ToString() });

	        }


            listRecent.ItemsSource = recentContextList;
            
            if (contexts != null)
            {
                listCloser.ItemsSource = closerContextList; 
            }

        }

        void _status_UserInteraction(object sender, EventArgs e)
        {
            UserInteractionEventArgs args = (UserInteractionEventArgs)e;

            switch (args.Code)
            {
                case UserInteractionCode.BetterContextFound:
                    break;
                case UserInteractionCode.CurrentContextFound:
                    break;
                case UserInteractionCode.NewContextSelected:
                    break;
                case UserInteractionCode.PreviousContextFound:
                    break;
                case UserInteractionCode.TrackingSessionSucceded:
                    break;
            }
        }

        void _status_StatusErrorNotification(object sender, StatusErrorNotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void UtilizeState(object state)
        {
            //throw new NotImplementedException();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // ToDO: to change returnUrl
            Switcher.Switch(new StartingPage());
        }

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
    }
}
