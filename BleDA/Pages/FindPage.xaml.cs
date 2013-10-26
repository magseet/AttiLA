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
using System.Threading;

namespace BleDA
{



    /// <summary>
    /// Interaction logic for FindPage.xaml
    /// </summary>
    /// 
    public partial class FindPage : UserControl, ISwitchable, ILocalizationServiceCallback
    {
        Status _status = Status.Instance;
        ContextService _contextService = new ContextService();
        System.Timers.Timer _refreshTimer = new System.Timers.Timer();
        Settings _settings = Settings.Instance;
        private LocalizationServiceClient _serviceClient;

        public FindPage()
        {
            InitializeComponent();
            var context = new InstanceContext(this);
            _serviceClient = new LocalizationServiceClient(context);

            _status.StatusErrorNotification += _status_StatusErrorNotification;
            _status.UserInteraction += _status_UserInteraction;

            // update in a separated thread to avoid UI freeze
            var t = new Thread(() => UpdateContextLists());
            t.Start();

            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
            _refreshTimer.Interval = _settings.FindRefreshInterval;
            _refreshTimer.Enabled = true;
        }

        /// <summary>
        /// User selected context
        /// </summary>
        string SelectedContextId
        {
            get
            {
                var recent = listRecent.SelectedItem as Context;
                var closer = listCloser.SelectedItem as ContextPreferenceItem;
                if(recent != null)
                {
                    return recent.Id.ToString();
                }
                else if(closer != null)
                {
                    return closer.ContextId.ToString();
                }
                return null;
            }
        }


        void _refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // suspend timer
            _refreshTimer.Stop();

            // check if user did select the context
            bool closerSelected = listCloser.Dispatcher.Invoke(
                new Func<bool>(
                    () => { return listCloser.SelectedItem != null; })
                );

            bool recentSelected = listRecent.Dispatcher.Invoke(
                new Func<bool>(
                    () => { return listRecent.SelectedItem != null; })
                );

            if (!recentSelected && !closerSelected && UpdateContextLists())
            {
                // resume timer
                _refreshTimer.Start();
            }
        }


        /// <summary>
        /// Populate the context lists.
        /// </summary>
        /// <returns>False if update has been skipped due to the user selection</returns>
        private bool UpdateContextLists()
        {
            // get new results

            var recentContexts = _contextService.GetMostRecent((int)_settings.MostRecentLimit);
            var closerContexts = _serviceClient.GetCloserContexts();

            List<ContextPreferenceItem> closerContextItems = new List<ContextPreferenceItem>();
            foreach (var closerContext in closerContexts.OrderByDescending(c => c.Value))
            {
                var context = _contextService.GetById(closerContext.ContextId);
                if (context == null)
                    continue;

                var item = new ContextPreferenceItem
                {
                    Name = context.ContextName,
                    Value = closerContext.Value.ToString(),
                    ContextId = closerContext.ContextId
                };

                closerContextItems.Add(item);

            }

            // update could take time
            // check again for user selection

            bool closerSelected = listCloser.Dispatcher.Invoke(
                new Func<bool>(
                    () => { return listCloser.SelectedItem != null; })
                );

            bool recentSelected = listRecent.Dispatcher.Invoke(
                new Func<bool>(
                    () => { return listRecent.SelectedItem != null; })
                );

            if (!recentSelected && !closerSelected)
            {
                // update UI

                listRecent.Dispatcher.Invoke(new System.Action(
                    () => { listRecent.ItemsSource = recentContexts; }));

                listCloser.Dispatcher.Invoke(new System.Action(
                    () => { listCloser.ItemsSource = closerContextItems; }));

                // update performed
                return true;
            }

            // update skipped
            return false;
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
            // TODO: somehow show progress..
        }

        public void ReportPrediction(string contextId)
        {
            // unused
        }

        public void ReportServiceStatus(ServiceStatus serviceStatus)
        {
            // usuned
        }

        private void listRecent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem != null)
            {
                // only one selection for both lists is showed
                listCloser.SelectedItem = null;
            }
        }

        private void listCloser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((sender as ListBox).SelectedItem != null)
            {
                // only one selection for both lists is showed
                listRecent.SelectedItem = null;
            }
        }

        private void btnLocalized_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedContextId == null)
            {
                // ToDO..
                MessageBox.Show("No selection");
            }
            else
            {
                // ToDO..
                MessageBox.Show("Selected: " + SelectedContextId);
            }
        }

    }
}
