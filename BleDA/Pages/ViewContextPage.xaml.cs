﻿using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using AttiLA.Data;
using MongoDB.Bson;
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
using System.ComponentModel;
using System.Collections.ObjectModel;
using BleDA.Actions;
using System.Diagnostics;
using MongoDB.Bson;
using Hardcodet.Wpf.TaskbarNotification;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for ViewContext.xaml
    /// </summary>
    public partial class ViewContextPage : UserControl, ISwitchable, INotifyPropertyChanged
    {
        private Status _status = Status.Instance;
        private TaskService _taskService = new TaskService();

        private ContextService _contextService = new ContextService();
        public IEnumerable<AttiLA.Data.Entities.Task> ExistingTasks
        {
            get
            {
                //SelectedTask = null;
                var tasks = _taskService.GetTaskDetails(int.MaxValue, 0);
                if (tasks == null)
                    return new List<AttiLA.Data.Entities.Task>();

                return tasks;
            }
        }

        public ObservableCollection<AttiLA.Data.Entities.Task> ContextTasks
        {
            get
            {
                //SelectedTask = null;
                if (string.IsNullOrWhiteSpace(_status.CurrentContextId))
                    return new ObservableCollection<AttiLA.Data.Entities.Task>();

                var contextTasks = _taskService.GetByContextId(_status.CurrentContextId);
                if (contextTasks == null)
                    return new ObservableCollection<AttiLA.Data.Entities.Task>();

                
                return new ObservableCollection<AttiLA.Data.Entities.Task>(contextTasks);
            }
        }

        public string SelectedContextName
        {
            get
            {
                var contextId = _status.CurrentContextId;
                if (contextId != null)
                {
                    var context = _contextService.GetById(contextId);
                    return context.ContextName;
                }
                return "";
            }
        }

        public AttiLA.Data.Entities.Task SelectedExistingTask { get; set; }

        public AttiLA.Data.Entities.Task SelectedContextTask { get; set; }

        public ViewContextPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void UtilizeState(object state)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new StartingPage());
        }

        private void btnExisting_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedExistingTask != null && !string.IsNullOrWhiteSpace(_status.CurrentContextId))
            {
                _taskService.AddContext(SelectedExistingTask.Id.ToString(), _status.CurrentContextId);
                
                if (null != this.PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs(Utils<ViewContextPage>.MemberName(s => s.ContextTasks)));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedContextTask != null)
            {
                _taskService.RemoveContext(SelectedContextTask.Id.ToString(), _status.CurrentContextId);
                if (null != this.PropertyChanged)
                    PropertyChanged(this, new PropertyChangedEventArgs(Utils<ViewContextPage>.MemberName(s => s.ContextTasks)));
            }                
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedContextTask != null)
            {
                Switcher.Switch(new TaskPage(), (object) SelectedContextTask);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ContextTasks == null)
                return;

            foreach( var task in ContextTasks ){
                if (task.Actions == null)
                    continue;

                foreach (var action in task.Actions)
                {
                    if (action is OpeningAction)
                    {
                        try
                        {
                            Processes.ExecuteProcess(action as OpeningAction);
                        }catch(Exception){
                            Debug.WriteLine("[BleDA] - Operation execute process skipped.");
                        }
                    }
                    else if (action is ClosingAction)
                    {

                    }
                    else if (action is NotificationAction)
                    {

                    }
                    else if (action is ServiceAction)
                    {

                    }
                }
            }
        }

        public string NewTaskName { get; set; }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewTaskName) && !string.IsNullOrWhiteSpace(_status.CurrentContextId))
            {
                var task = new AttiLA.Data.Entities.Task
                {
                    CreationDateTime = DateTime.Now,
                    TaskName = NewTaskName
                };
                task.Contexts.Add(new ObjectId(_status.CurrentContextId));
                try
                {
                    _taskService.Create(task);
                }
                catch
                {
                    _status.NotifyIcon.ShowBalloonTip(Properties.Resources.PopupWarning, "Error during task creation.", BalloonIcon.Warning);
                }

                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(Utils<ViewContextPage>.MemberName(s => s.ContextTasks)));
                    PropertyChanged(this, new PropertyChangedEventArgs(Utils<ViewContextPage>.MemberName(s => s.ExistingTasks)));
                    NewTaskName = "";
                    PropertyChanged(this, new PropertyChangedEventArgs(Utils<ViewContextPage>.MemberName(s => s.NewTaskName)));
                }
            }

            
        }
    }
}
