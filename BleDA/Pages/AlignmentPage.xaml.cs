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
using BleDA.LocalizationService;
using System.ServiceModel;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using System.Diagnostics;
using System.ComponentModel;
using AttiLA.Data;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for AlignmentPage.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false,
        ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class AlignmentPage : UserControl, ISwitchable, IDisposable, INotifyPropertyChanged
    {
        Status _status = Status.Instance;
        ContextService _contextService = new ContextService();

        public AlignmentPage()
        {
            InitializeComponent();
            DataContext = this;
            _status.Updated += _status_Updated;
        }

        void _status_Updated()
        {
            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(
                    Utils<AlignmentPage>.MemberName(p => p.SelectedContextName)));
                PropertyChanged(this, new PropertyChangedEventArgs(
                    Utils<AlignmentPage>.MemberName(p => p.ServiceStatus)));
                PropertyChanged(this, new PropertyChangedEventArgs(
                    Utils<AlignmentPage>.MemberName(p => p.ServiceContextName)));
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

        public string ServiceContextName
        {
            get
            {
                var serviceStatus = _status.ServiceStatus;
                if (serviceStatus == null)
                {
                    return "None";
                }
                var contextId = _status.ServiceStatus.ContextId;
                if (contextId != null)
                {
                    var context = _contextService.GetById(contextId);
                    return context.ContextName;
                }
                return "";
            }
        }

        public string ServiceStatus
        {
            get
            {
                var serviceStatus = _status.ServiceStatus;
                if (serviceStatus == null)
                {
                    return "Not responding";
                }
                return _status.ServiceStatus.ServiceState.ToString();
            }
        }

        public void UtilizeState(object state)
        {
            //throw new NotImplementedException();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // ToDO: to change returnUrl
            Switcher.Switch(new StartingPage());
        }


        #region IDisposable implementation
        public void Dispose()
        {
            Dispose(true);              //i am calling you from Dispose, it's safe
            GC.SuppressFinalize(this);  //Hey, GC: don't bother calling finalize later
        }

        protected void Dispose(Boolean freeManagedObjectsAlso)
        {
            //Free unmanaged resources
            _status.Updated -= _status_Updated;

            //Free managed resources too, but only if i'm being called from Dispose
            //(If i'm being called from Finalize then the objects might not exist
            //anymore
            if (freeManagedObjectsAlso)
            {
            }
        }

        ~AlignmentPage()
        {
            Dispose(false); //i am *not* calling you from Dispose, it's *not* safe
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
