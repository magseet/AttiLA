﻿using BleDA.LocalizationService;
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
using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using MongoDB.Bson;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for StartingPage.xaml
    /// </summary>
    public partial class StartingPage : UserControl, ISwitchable
    {
        Status _status = Status.Instance;
        ContextService _contextService = new ContextService();

        public StartingPage()
        {
            InitializeComponent();

            _status.UserInteraction += _status_UserInteraction;
            _status.StatusErrorNotification += _status_StatusErrorNotification;

            _status.Initialize();


        }

        void _status_StatusErrorNotification(object sender, StatusErrorNotificationEventArgs e)
        {
            StatusErrorNotificationEventArgs args = (StatusErrorNotificationEventArgs)e;
            switch (args.Code)
            {
                case StatusErrorNotificationCode.TrackingSessionFailed:
                    break;
                case StatusErrorNotificationCode.UnexpectedPrediction:
                    break;
            }
        }

        void _status_UserInteraction(object sender, EventArgs e)
        {
            UserInteractionEventArgs args = (UserInteractionEventArgs) e;

            switch(args.Code){
                case UserInteractionCode.BetterContextFound:
                    break;
                case UserInteractionCode.CurrentContextFound:
                    break;
                case UserInteractionCode.NewContextSelected:
                    break;
                case UserInteractionCode.PreviousContextFound:
                    var context = _contextService.GetById(args.PreviousContextFoundValue);
                    if (context != null)
                    {
                        MessageBox.Show("Attila è attivo. Context: " + context.ContextName);
                    }
                    break;
                case UserInteractionCode.TrackingSessionSucceded:
                    break;
            }
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
            Switcher.Switch(new FindPage(), new { returnPage = "Starting" });
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
