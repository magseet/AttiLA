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
using BleDA.LocalizationService;
using System.ServiceModel;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;
using System.Diagnostics;
using Ookii.Dialogs.Wpf;
using Ookii.Dialogs;
using Hardcodet.Wpf.TaskbarNotification;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Status _status = Status.Instance;
        private ContextService _contextService = new ContextService();
        
        public MainWindow()
        {
            InitializeComponent();
            //initialize NotifyIcon
            _status.NotifyIcon = (TaskbarIcon)FindResource("AttiLANotifyIcon");

            // initialize status
            _status.UserInteraction += _status_UserInteraction;
            _status.StatusErrorNotification += _status_StatusErrorNotification;
            _status.Initialize();

            Switcher.pageSwitcher = this;
            Switcher.Switch(new StartingPage());
        }

        void _status_StatusErrorNotification(object sender, StatusErrorNotificationEventArgs e)
        {
            var args = e as StatusErrorNotificationEventArgs;
            Debug.WriteLine("[BleDA] StatusErrorNotification: " + args.Code.ToString());
            switch (args.Code)
            {
                case StatusErrorNotificationCode.UnexpectedPrediction:
                    break;
                case StatusErrorNotificationCode.TrackingSessionFailed:
                    MessageBox.Show("Alignment failed");
                    break;
                default:
                    break;
            }
        }


        void _status_UserInteraction(object sender, EventArgs e)
        {
            var args = e as UserInteractionEventArgs;
            Debug.WriteLine("[BleDA] UserInteraction: " + args.Code.ToString());
            switch (args.Code)
            {
                case UserInteractionCode.BetterContextFound:
                    MessageBox.Show("Better context found");
                    if (MessageBox.Show(
                        "Confirm your position?",
                        "Better context found",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning
                        ) == MessageBoxResult.No)
                    {
                        // permit new context selection
                        this.Dispatcher.BeginInvoke(new System.Action(
                        () => { Switcher.Switch(new FindPage()); }));
                        break;

                    }
                    else
                    {
                        // send confirmation
                        _status.ContextSelected(_status.CurrentContextId);
                    }

                    break;
                case UserInteractionCode.CurrentContextFound:
                    _status.NotifyIcon.ShowBalloonTip(Properties.Resources.PopupInfo,
                        Properties.Resources.MsgAlignmentCompleted, BalloonIcon.Info);

                    this.Dispatcher.BeginInvoke(new System.Action(
                        () => { Switcher.Switch(new ViewContextPage()); }));
                    break;

                case UserInteractionCode.NewContextSelected:

                    _status.NotifyIcon.ShowBalloonTip(Properties.Resources.PopupInfo,
                        Properties.Resources.MsgAlignmentStarted, BalloonIcon.Info);
                    //_notifyIcon.HideBalloonTip();

                    this.Dispatcher.BeginInvoke(new System.Action(
                        () => { Switcher.Switch(new AlignmentPage()); }));
                    break;

                case UserInteractionCode.PreviousContextFound:
                    var context = _contextService.GetById(args.PreviousContextFoundValue);
                    _status.NotifyIcon.ShowBalloonTip(Properties.Resources.PopupInfo,
                        Properties.Resources.MsgPreviousContext + context.ContextName, BalloonIcon.Info);
                    //_notifyIcon.HideBalloonTip();

                    this.Dispatcher.BeginInvoke(new System.Action(
                        () => { Switcher.Switch(new ViewContextPage()); }));
                    break;

                case UserInteractionCode.TrackingSessionSucceded:
                    _status.NotifyIcon.ShowBalloonTip(Properties.Resources.PopupInfo,
                        Properties.Resources.MsgContexUpdated, BalloonIcon.Info);
                    break;
            }
        }

        public void Navigate(UserControl nextPage)
        {
            var prevPage = this.Content;
            if (prevPage is IDisposable)
            {
                (prevPage as IDisposable).Dispose();
            }
            this.Content = nextPage;

        }

        public void Navigate(UserControl nextPage, object state)
        {
            var prevPage = this.Content;
            if (prevPage is IDisposable)
            {
                (prevPage as IDisposable).Dispose();
            }
            this.Content = nextPage;
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
                s.UtilizeState(state);
            else
                throw new ArgumentException("NextPage is not ISwitchable! "
                  + nextPage.Name.ToString());
        }
    }
}
