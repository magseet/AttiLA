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
        private Settings _settings;

        public SettingsPage()
        {
            InitializeComponent();

            try
            {
                _settings = Settings.Instance;

                if (_settings.Service != null)
                
                {
                    txtNotificationThreshold.Text = _settings.Service.NotificationThreshold.ToString();

                    //Localizer import settings
                    txtLocalizerInterval.Text = _settings.Service.Localizer.Interval.ToString();
                    txtLocalizerReties.Text = _settings.Service.Localizer.Retries.ToString();
                    
                    // fill combo box with this..
                    //_settings.Service.Localizer.SimilarityAlgorithm

                    //Tracker import settings
                    txtTrackerInverval.Text = _settings.Service.Tracker.Interval.ToString();
                    txtTrackerTrainingThreshold.Text = _settings.Service.Tracker.TrainingThreshold.ToString();
                }
                    

            }
            catch(SettingsException)
            {
                //..
            }
            
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
            uint notificationThreshold, retries;
            double interval;
            if (UInt32.TryParse(txtNotificationThreshold.Text, out notificationThreshold)){
                _settings.Service.NotificationThreshold = notificationThreshold;
            }

            if (UInt32.TryParse(txtLocalizerReties.Text, out retries))
            {
                _settings.Service.Localizer.Retries = retries;
            }

        }
    }
}
