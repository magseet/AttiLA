﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AttiLA.Test.LocalizationService.LocalizationServiceReference;
using System.ServiceModel;

namespace AttiLA.Test.LocalizationService
{
    public partial class Form1 : Form
    {
        private LocalizationServiceClient serviceClient;
        
        public Form1()
        {
            InitializeComponent();
            serviceClient = new LocalizationServiceClient(
                Properties.Settings.Default.EndpointConfigurationName);
        }

        private void buttonChangeContextId_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(textBoxContextId.Text))
            {
                try
                {
                    serviceClient.ChangeContext(textBoxContextId.Text);
                }
                catch(FaultException<ArgumentException>)
                {
                    MessageBox.Show("invalid context");
                }
                catch(FaultException<ServiceException> ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch
                {
                    MessageBox.Show("PD!");
                }
            }
            
        }
    }
}
