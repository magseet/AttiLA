using BleDA.LocalizationService;
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
    /// Interaction logic for StartingPage.xaml
    /// </summary>
    public partial class StartingPage : UserControl, ISwitchable
    {        

        public StartingPage()
        {
            InitializeComponent();
            Status.Instance.Initialize();
        }

        public void UtilizeState(object state)
        {
            //throw new NotImplementedException();
        }

        private void btnGoContext_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ContextPage());
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ManagedProfilePage(), new { returnPage = "Starting" });
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new SettingsPage(), new { returnPage = "Starting" });
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AboutPage(), new { returnPage = "Starting" });
        }
    }
}
