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
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;

namespace BleDA
{
    public enum Operation
    {
        [Description("Start program")]
        Start = 1,
        [Description("Stop program")]
        Stop,
        [Description("Manage windows service")]
        Service,
        [Description("Send notification")]
        Notification

    }
    /// <summary>
    /// Interaction logic for ManagedProfilePage.xaml
    /// </summary>
    public partial class TaskPage : UserControl, ISwitchable, INotifyPropertyChanged
    {
        private TaskService _taskService = new TaskService();
        private String _openingActionPath;
        private String _openingActionArgument;

        public String OpeningActionPath 
        {
            get 
            { 
                return _openingActionPath; 
            }  
            set 
            {
                _openingActionPath = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("OpeningActionPath"));
                }
            }
        }
        public String OpeningActionArguments 
        {
            get 
            { 
                return _openingActionArgument; 
            }  
            set 
            {
                _openingActionArgument = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("OpeningActionPath"));
                }
            }
        }

        public AttiLA.Data.Entities.Task SelectedTask { get; set; }

        public String TaskName { 
            get 
            { 
                var task = _taskService.GetById(SelectedTask.Id.ToString()); 
                if(task == null)
                    return "";

                return task.TaskName;
            }
        }

        public IEnumerable<AttiLA.Data.Entities.Action> TaskActions
        {
            get
            {
                if (SelectedTask != null)
                {
                    var task = _taskService.GetActions(SelectedTask.Id.ToString());
                    if (task == null)
                        return new List<AttiLA.Data.Entities.Action>();

                    return task;
                }

                return new List<AttiLA.Data.Entities.Action>();
            }
        }

        public TaskPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void UtilizeState(object state)
        {
            SelectedTask = (AttiLA.Data.Entities.Task)state;
        }

        //private AttiLA.Data.Entities.Task _selectedAction;
        //private AttiLA.Data.Entities.Task SelectedAction
        //{
        //    get
        //    {
        //        return _selectedAction;
        //    }
        //    set
        //    {
        //        if (_selectedAction != value)
        //        {
        //            _selectedAction = value;
        //            if (value != null)
        //            {
        //                _selectedAction.Actions = _taskService
        //                    .GetActions(value.Id.ToString())
        //                    .ToList();
        //            }
        //            OnSelectedTaskChanged();
        //        }
        //    }
        //}

        //public void OnSelectedTaskChanged()
        //{
        //    List<Action> listActions = new List<Action>();
        //    taskSelected.Items.Clear();
        //    if (SelectedTask == null)
        //    {
        //        return;
        //    }

        //    foreach (var action in SelectedTask.Actions)
        //    {
        //        //lvi.Text = action.GetType().ToString();

        //        if (action.GetType() == typeof(OpeningAction))
        //        {
        //            var openingAction = (OpeningAction)action;
        //            listActions.Add(new Action { Operation = Operation.Start, ActionName = action.GetType().ToString(), Param1 = openingAction.Path, Param2 = openingAction.Arguments });
        //        }
        //        else if (action.GetType() == typeof(ClosingAction))
        //        {
        //            var closingAction = (ClosingAction)action;
        //            listActions.Add(new Action { Operation = Operation.Stop, ActionName = action.GetType().ToString(), Param1 = closingAction.ExecutablePath, Param2 = closingAction.CommandLine });

        //        }
        //        else if (action.GetType() == typeof(ServiceAction))
        //        {
        //            var serviceAction = (ServiceAction)action;
        //            //lvi.SubItems.Add(serviceAction.ServiceName);
        //            //lvi.SubItems.Add(serviceAction.ServiceActionType.ToString());
        //        }
        //        else if (action.GetType() == typeof(NotificationAction))
        //        {
        //            var notificationAction = (NotificationAction)action;
        //            //lvi.SubItems.Add(notificationAction.Message);
        //        }
        //    }

        //    taskSelected.ItemsSource = listActions;
        //}

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // ToDO: to change returnUrl
            Switcher.Switch(new ViewContextPage());
        }

        private String ShowOpenFileDialog()
        {
            // As of .Net 3.5 SP1, WPF's Microsoft.Win32.OpenFileDialog class still uses the old style
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*";
            
            if (!VistaFileDialog.IsVistaFileDialogSupported)
                MessageBox.Show("Because you are not using Windows Vista or later, the regular open file dialog will be used. Please use Windows Vista to see the new dialog.", "Sample open file dialog");
            if ((bool)dialog.ShowDialog())
                return dialog.FileName;

            return "";
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            String retValue = ShowOpenFileDialog();
            OpeningActionPath = retValue;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(OpeningActionPath);
            OpeningActionArguments = "";
            OpeningActionPath = "";
        }

        private void btnAddActionStart_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
