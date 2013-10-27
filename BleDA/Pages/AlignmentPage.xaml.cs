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
using BleDA.LocalizationService;
using System.ServiceModel;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using System.Diagnostics;

namespace BleDA
{
    /// <summary>
    /// Interaction logic for AlignmentPage.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false,
        ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class AlignmentPage : UserControl, ISwitchable
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
            this.Dispatcher.Invoke(new System.Action(
                () => {

                    txtStatus.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                    txtSelectedContext.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                    txtServiceContext.GetBindingExpression(TextBox.TextProperty).UpdateTarget(); 
                }));
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
    }
}
