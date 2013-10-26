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

namespace BleDA
{
    /// <summary>
    /// Interaction logic for ContextPage.xaml
    /// </summary>
    public partial class NewContextPage : UserControl, ISwitchable
    {
        public NewContextPage()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void btnCreateContext_Click(object sender, RoutedEventArgs e)
        {
            if(txtContextName.Text != ""){
                // Status.Instance.CurrentContextId = txtContextName.Text;

                Switcher.Switch(new StartingPage());
                /*try
                {
                    _contextService.Create(c);
                }
                catch (DatabaseException)
                {
                    MessageBox.Show("Already exists.");
                    return;
                }*/               
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new StartingPage());
        }

    }
}
