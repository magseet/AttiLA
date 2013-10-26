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
using BleDA.LocalizationService;
using System.ServiceModel;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using System.Diagnostics;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for AlignmentPage.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false,
        ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class AlignmentPage : UserControl, ISwitchable
    {
        Status _status = Status.Instance;
        ContextService _contextService = new ContextService();
        Settings _settings = Settings.Instance;

        public AlignmentPage()
        {
            InitializeComponent();

            _status.StatusErrorNotification += _status_StatusErrorNotification;
            _status.UserInteraction += _status_UserInteraction;
        }

        void _status_UserInteraction(object sender, EventArgs e)
        {
            //var args = e as UserInteractionEventArgs;
            //Debug.WriteLine("[Find] UserInteraction: " + args.Code.ToString());
            //switch (args.Code)
            //{
            //    case UserInteractionCode.BetterContextFound:
            //        MessageBox.Show("You moved away?");
            //        break;
            //    case UserInteractionCode.CurrentContextFound:
            //        MessageBox.Show("Alignment completed.");
            //        break;
            //    case UserInteractionCode.NewContextSelected:
            //        MessageBox.Show("Alignment started. Please don't move.");
            //        break;
            //    case UserInteractionCode.PreviousContextFound:
            //        break;
            //    case UserInteractionCode.TrackingSessionSucceded:
            //        MessageBox.Show("Context updated with new data.");
            //        break;
            //}
        }

        private void _status_StatusErrorNotification(object sender, StatusErrorNotificationEventArgs e)
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
    }
}
