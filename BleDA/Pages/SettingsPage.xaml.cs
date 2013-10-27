using BleDA.LocalizationService;
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

namespace BleDA
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl, ISwitchable
    {
        private Settings _settings = Settings.Instance;
        private Status _status = Status.Instance;

        public SettingsPage()
        {
            InitializeComponent();
            DataContext = _settings.Service;

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

        private void btnCreateContext_Click(object sender, RoutedEventArgs e)
        {
            uint notificationThreshold, retries, localizerInterval, trackerInterval, TrackerTrainingThreshold;
            GlobalSettings serviceSettings = new GlobalSettings() { Localizer = new LocalizerSettings(), Tracker = new TrackerSettings() };

            if (UInt32.TryParse(txtNotificationThreshold.Text, out notificationThreshold))
            {
                serviceSettings.NotificationThreshold = notificationThreshold;
            }

            if (UInt32.TryParse(txtLocalizerReties.Text, out retries))
            {
                serviceSettings.Localizer.Retries = retries;
            }

            if (UInt32.TryParse(txtLocalizerInterval.Text, out localizerInterval))
            {
                serviceSettings.Localizer.Interval = localizerInterval;
            }

            if (UInt32.TryParse(txtTrackerInverval.Text, out trackerInterval))
            {
                serviceSettings.Localizer.Retries = retries;
            }

            if (UInt32.TryParse(txtTrackerTrainingThreshold.Text, out TrackerTrainingThreshold))
            {
                serviceSettings.Localizer.Retries = retries;
            }

            serviceSettings.Localizer.SimilarityAlgorithm = _settings.Service.Localizer.SimilarityAlgorithm;

            try
            {
                _settings.Service = serviceSettings;
            }
            catch (Exception)
            {
                _status.NotifyIcon.ShowBalloonTip(Properties.Resources.PopupWarning, "Update settings failed.", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Warning);
            }

            Switcher.Switch(new StartingPage());
        }
    }
}
