﻿using System;
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

namespace BleDA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Status _status = Status.Instance;
        ContextService _contextService = new ContextService();

        public MainWindow()
        {
            InitializeComponent();
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
                    MessageBox.Show("Alignment completed.");
                    this.Dispatcher.BeginInvoke(new System.Action(
                        () => { Switcher.Switch(new ManagedProfilePage()); }));
                    break;

                case UserInteractionCode.NewContextSelected:
                    MessageBox.Show("Alignment started. Please don't move.");
                    this.Dispatcher.BeginInvoke(new System.Action(
                        () => { Switcher.Switch(new AlignmentPage()); }));
                    break;

                case UserInteractionCode.PreviousContextFound:
                    var context = _contextService.GetById(args.PreviousContextFoundValue);
                    if (context != null)
                    {
                        MessageBox.Show("Attila è attivo. Context: " + context.ContextName);
                    }
                    break;
                case UserInteractionCode.TrackingSessionSucceded:
                    MessageBox.Show("Context updated with new data.");
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
