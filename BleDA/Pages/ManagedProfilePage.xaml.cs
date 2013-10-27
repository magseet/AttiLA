﻿using System;
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
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;

namespace BleDA
{
    public enum Operation
    {
        [Description("Start")]
        Start = 1,
        [Description("Stop")]
        Stop
    }
    /// <summary>
    /// Interaction logic for ManagedProfilePage.xaml
    /// </summary>
    public partial class ManagedProfilePage : UserControl, ISwitchable
    {
        private TaskService _taskService = new TaskService();
        public ManagedProfilePage()
        {
            InitializeComponent();
            this.DataContext = new EnumHelper();
        }

        public void UtilizeState(object state)
        {
            //throw new NotImplementedException();
        }

        private AttiLA.Data.Entities.Task _selectedTask;
        private AttiLA.Data.Entities.Task SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                if (_selectedTask != value)
                {
                    _selectedTask = value;
                    if (value != null)
                    {
                        _selectedTask.Actions = _taskService
                            .GetActions(value.Id.ToString())
                            .ToList();
                    }
                    OnSelectedTaskChanged();
                }
            }
        }

        public void OnSelectedTaskChanged()
        {
            List<Action> listActions = new List<Action>();
            taskSelected.Items.Clear();
            if (SelectedTask == null)
            {
                return;
            }

            foreach (var action in SelectedTask.Actions)
            {
                //lvi.Text = action.GetType().ToString();

                if (action.GetType() == typeof(OpeningAction))
                {
                    var openingAction = (OpeningAction)action;
                    listActions.Add(new Action { Operation = Operation.Start, ActionName = action.GetType().ToString(), Param1 = openingAction.Path, Param2 = openingAction.Arguments });
                }
                else if (action.GetType() == typeof(ClosingAction))
                {
                    var closingAction = (ClosingAction)action;
                    listActions.Add(new Action { Operation = Operation.Stop, ActionName = action.GetType().ToString(), Param1 = closingAction.ExecutablePath, Param2 = closingAction.CommandLine });

                }
                else if (action.GetType() == typeof(ServiceAction))
                {
                    var serviceAction = (ServiceAction)action;
                    //lvi.SubItems.Add(serviceAction.ServiceName);
                    //lvi.SubItems.Add(serviceAction.ServiceActionType.ToString());
                }
                else if (action.GetType() == typeof(NotificationAction))
                {
                    var notificationAction = (NotificationAction)action;
                    //lvi.SubItems.Add(notificationAction.Message);
                }
            }

            taskSelected.ItemsSource = listActions;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // ToDO: to change returnUrl
            Switcher.Switch(new StartingPage());
        }

        private void ShowOpenFileDialog(string operation)
        {
            // As of .Net 3.5 SP1, WPF's Microsoft.Win32.OpenFileDialog class still uses the old style
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            switch (operation)
            {
                case "Start":
                case "Stop":
                    dialog.Filter = "*.exe";
                    break;
                default:
                    dialog.Filter = "All files (*.*)|*.*";
                    break;
            }
            if (!VistaFileDialog.IsVistaFileDialogSupported)
                MessageBox.Show("Because you are not using Windows Vista or later, the regular open file dialog will be used. Please use Windows Vista to see the new dialog.", "Sample open file dialog");
            if ((bool)dialog.ShowDialog())
                MessageBox.Show("The selected file was: " + dialog.FileName, "Sample open file dialog");
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var operation = comboBoxAction.SelectedValue;
            ShowOpenFileDialog(operation.ToString());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
