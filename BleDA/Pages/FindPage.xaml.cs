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
using System.Diagnostics;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for FindPage.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false,
        ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class FindPage : UserControl, ISwitchable, IDisposable, ILocalizationServiceCallback
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

            // update in a separated thread to avoid UI freeze
            ThreadPool.QueueUserWorkItem(new WaitCallback(
                (object o) =>  UpdateContextLists()
                ));

            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
            _refreshTimer.Interval = _settings.FindRefreshInterval;
            _refreshTimer.Enabled = true;

            _serviceClient.Subscribe();
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
                if (recent != null)
                {
                    return recent.Id.ToString();
                }
                else if (closer != null)
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

            try
            {
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

            }
            catch
            {
                // tried to access UI after Dispose()
                return false;
            }

            // update skipped
            return false;
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
            // Debug.WriteLine("[Find] Progress: " + progress);
        }

        public void ReportPrediction(string contextId)
        {
            // unused
        }

        public void ReportServiceStatus(ServiceStatus serviceStatus)
        {
            // unused
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
            if ((sender as ListBox).SelectedItem != null)
            {
                // only one selection for both lists is showed
                listRecent.SelectedItem = null;
            }
        }

        /// <summary>
        /// User made a selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLocalized_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedContextId == null)
            {
                Debug.WriteLine("[Find] No selection");
                MessageBox.Show("No selection");
            }
            else
            {
                Debug.WriteLine("[Find] Selection: " + SelectedContextId);
                _status.ContextSelected(SelectedContextId);
                Switcher.Switch(new ViewContextPage());
            }
        }

        #region IDisposable implementation
        public void Dispose()
        {
            Dispose(true);              //i am calling you from Dispose, it's safe
            GC.SuppressFinalize(this);  //Hey, GC: don't bother calling finalize later
        }

        protected void Dispose(Boolean freeManagedObjectsAlso)
        {
            //Free unmanaged resources
            _serviceClient.Unsubscribe();

            //Free managed resources too, but only if i'm being called from Dispose
            //(If i'm being called from Finalize then the objects might not exist
            //anymore
            if (freeManagedObjectsAlso)
            {
                _refreshTimer.Enabled = false;
            }
        }

        ~FindPage()
        {
            Dispose(false); //i am *not* calling you from Dispose, it's *not* safe
        }
        #endregion
    }
}
