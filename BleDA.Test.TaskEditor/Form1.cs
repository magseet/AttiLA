using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ookii.Dialogs;
using AttiLA.Data.Entities;
using AttiLA.Data.Services;
using BleDA.Actions;

namespace BleDA.Test.TaskEditor
{

    public partial class Form1 : Form
    {
        TaskService _taskService = new TaskService();

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

        public Form1()
        {
            InitializeComponent();
            UpdateTaskList();
        }

        public void UpdateTaskList()
        {
            _tasksCommonBox.Items.Clear();
            SelectedTask = null;
            var tasks = _taskService.GetTaskDetails(int.MaxValue, 0);
            foreach(var task in tasks)
            {
                var item = new ComboBoxItem<AttiLA.Data.Entities.Task>(task.TaskName, task);
                _tasksCommonBox.Items.Add(item);
            }
        }

        public void OnSelectedTaskChanged()
        {
            _actionListView.Items.Clear();
            if(SelectedTask == null)
            {
                return;
            }

            foreach (var action in SelectedTask.Actions)
            {
                var lvi = new ListViewItem();
                lvi.Text = action.GetType().ToString();

                if (action.GetType() == typeof(OpeningAction))
                {
                    var openingAction = (OpeningAction)action;
                    lvi.SubItems.Add(openingAction.Path);
                    lvi.SubItems.Add(openingAction.Arguments);
                }
                else if (action.GetType() == typeof(ClosingAction))
                {
                    var closingAction = (ClosingAction)action;
                    lvi.SubItems.Add(closingAction.ProcessName);
                    lvi.SubItems.Add(closingAction.ExecutablePath);
                    lvi.SubItems.Add(closingAction.CommandLine);
                }
                else if (action.GetType() == typeof(ServiceAction))
                {
                    var serviceAction = (ServiceAction)action;
                    lvi.SubItems.Add(serviceAction.ServiceName);
                    lvi.SubItems.Add(serviceAction.ServiceActionType.ToString());
                }
                else if (action.GetType() == typeof(NotificationAction))
                {
                    var notificationAction = (NotificationAction)action;
                    lvi.SubItems.Add(notificationAction.Message);
                }
                _actionListView.Items.Add(lvi);
            }
        }

        private void _createButton_Click(object sender, EventArgs e)
        {
            InputDialog dialog = new InputDialog
            {
                WindowTitle = "Create new task",
                Content = "Insert the task name:"
            };
            var result = dialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                var taskName = dialog.Input;
                if(string.IsNullOrWhiteSpace(taskName))
                {
                    MessageBox.Show("Operation aborted.");
                }
                else
                {
                    var task = new AttiLA.Data.Entities.Task
                    {
                        TaskName = taskName,
                        CreationDateTime = DateTime.Now
                    };
                    _taskService.Create(task);
                    UpdateTaskList();
                }
                
            }
        }

        private void _tasksCommonBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (ComboBoxItem<AttiLA.Data.Entities.Task>)_tasksCommonBox.SelectedItem;
            SelectedTask = item.HiddenValue;
        }
    }
}
