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
using AttiLA.Data;
using System.Collections.ObjectModel;

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
        private Status _status = Status.Instance;
        private ObservableCollection<AttiLA.Data.Entities.Action> _taskActions = new ObservableCollection<AttiLA.Data.Entities.Action>();

        public String OpeningActionPath
        {
            get
            {
                return string.IsNullOrWhiteSpace(_openingActionPath) ? null : _openingActionPath;
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
                return string.IsNullOrWhiteSpace(_openingActionArgument) ? null : _openingActionArgument;
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

        public AttiLA.Data.Entities.Action SelectedAction { get; set; }

        public String TaskName
        {
            get
            {
                var task = _taskService.GetById(SelectedTask.Id.ToString());
                if (task == null)
                    return "";

                return task.TaskName;
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
            LoadActions();
            taskSelected.ItemsSource = _taskActions;
        }

        private void LoadActions()
        {
            if (SelectedTask != null)
            {
                var actions = _taskService.GetActions(SelectedTask.Id.ToString());
                if (actions == null)
                    _taskActions = new ObservableCollection<AttiLA.Data.Entities.Action>();
                else
                    _taskActions = new ObservableCollection<AttiLA.Data.Entities.Action>(actions);
            }
            else
            {
                _taskActions = new ObservableCollection<AttiLA.Data.Entities.Action>();
            }
            
        }

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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(OpeningActionPath);
            OpeningActionArguments = "";
            OpeningActionPath = "";
        }

        private void btnAddActionStart_Click(object sender, RoutedEventArgs e)
        {
            if (OpeningActionPath == null || SelectedTask == null)
                return;

            var action = new OpeningAction { Path = OpeningActionPath, Arguments = OpeningActionArguments };

            _taskActions.Add(action);
        }

        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask == null)
                return;

            SelectedTask.Actions = _taskActions.ToList();

            try
            {
                _taskService.Update(SelectedTask);
            }
            catch (DatabaseException)
            {
                _status.NotifyIcon.ShowBalloonTip(Properties.Resources.PopupWarning, "Update failed. Try again later", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Warning);
            }

            Switcher.Switch(new ViewContextPage());
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAction != null)
                _taskActions.Remove(SelectedAction);
        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAction != null)
            {
                int indexAction = _taskActions.IndexOf(SelectedAction);
                if (indexAction > 0)
                    _taskActions.Move(indexAction, indexAction - 1);
            }
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAction != null)
            {
                int indexAction = _taskActions.IndexOf(SelectedAction);
                if (indexAction < _taskActions.Count - 1)
                    _taskActions.Move(indexAction, indexAction + 1);
            }
        }
    }
}
