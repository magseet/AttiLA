﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;

namespace AttiLA.Test.CreateContext
{
    public partial class Form1 : Form
    {
        ContextService contextService;

        string ContextName { 
            get
            {
                return textBoxName.Text;
            }
            set
            {
                button1.Enabled = !String.IsNullOrWhiteSpace(value);
            }
        }

        public Form1()
        {
            InitializeComponent();
            contextService = new ContextService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Context c = new Context
            {
                ContextName = this.ContextName,
                CreationDateTime = DateTime.Now
            };
            contextService.Create(c);
            button1.Enabled = false;
            textBoxName.Enabled = false;
            labelID.Text = c.Id.ToString();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            ContextName = textBoxName.Text;
        }
    }
}
