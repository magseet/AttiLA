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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl, ISwitchable
    {
        private AttiLA.GlobalSettings _settings;
        public Settings()
        {
            InitializeComponent();
            
            if(_settings.NotificationThreshold != null)
                txtNotificationThreshold.Text = _settings.NotificationThreshold.ToString();
            
            //Localizer import settings
            if(_settings.Localizer.Interval != null)
                txtLocalizerInterval.Text = _settings.Localizer.Interval.ToString();
            
            if(_settings.Localizer.Retries != null)
                txtLocalizerReties.Text = _settings.Localizer.Retries.ToString();

            //Tracker import settings
            if (_settings.Tracker.Interval != null)
                txtTrackerInverval.Text = _settings.Tracker.Interval.ToString();

            if (_settings.Tracker.TrainingThreshold != null)
                txtTrackerTrainingThreshold.Text = _settings.Tracker.TrainingThreshold.ToString();
        }

        public void UtilizeState(object state)
        {
            
            //throw new NotImplementedException();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // ToDO: to change returnUrl
            Switcher.Switch(new Starting());
        }

        private void btnCreateContext_Click(object sender, RoutedEventArgs e)
        {
            uint notificationThreshold, retries;
            double interval;
            if (UInt32.TryParse(txtNotificationThreshold.Text, out notificationThreshold)){
                _settings.NotificationThreshold = notificationThreshold;
            }

            if (UInt32.TryParse(txtLocalizerReties.Text, out retries))
            {
                _settings.Localizer.Retries = retries;
            }

        }
    }
}
