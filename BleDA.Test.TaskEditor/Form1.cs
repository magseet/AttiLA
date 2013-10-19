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

        public Form1()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, EventArgs e)
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
                        TaskName = taskName
                    };
                    _taskService.Create(task);
                }
                
            }
        }
    }
}
