using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using AttiLA.Test.LocalizationService.LocalizationServiceReference;
using System.ServiceModel;
using AttiLA.Data.Services;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using System.Diagnostics;
using NativeWifi;

namespace AttiLA.Test.LocalizationService
{
    [CallbackBehavior(
        //ConcurrencyMode = ConcurrencyMode.Reentrant
        UseSynchronizationContext=false
        )]
    public partial class Form1 : Form, ILocalizationServiceCallback
    {
        #region Private members

        System.IO.StreamWriter _logFile = new System.IO.StreamWriter("log.txt", true)
        {
            AutoFlush = true
        };

        private LocalizationServiceClient _serviceClient;

        private ContextService _contextService = new ContextService();

        private SynchronizationContext _syncContext = null;

        private WlanClient _wlanClient = new WlanClient();

        private string SelectedContext { get; set; }

        #endregion

        public delegate TV MyFunc<in T, TU, out TV>(T input, out TU output);

        public Form1()
        {
            InitializeComponent();
            _logFile.WriteLine(" -----------------------------------------------------------");
            _syncContext = WindowsFormsSynchronizationContext.Current;
            var context = new InstanceContext(this);
            _serviceClient = new LocalizationServiceClient(context);
            _serviceClient.Subscribe();
            //Context c =_contextService.GetById("ciao");
            //foreach (var networkInterface in c.NetworkInterfaces)
            //{
            //    var action = networkInterface.Action;
            //    if(action == NetworkInterfaceActionType.Connect)
            //    {
            //        var profile = networkInterface.ProfileName;

            //        var wlanIface = _wlanClient.Interfaces
            //            .AsQueryable()
            //            .Where(i => i.InterfaceGuid.ToString() == networkInterface.InterfaceGuid)
            //            .SingleOrDefault();

            //        if(wlanIface == null)
            //        {
            //            // may be removed
            //            continue;
            //        }
            //        wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profile);
            //    } 
            //    else if(action == NetworkInterfaceActionType.Disconnect)
            //    {
            //       //...
            //    }
            //}
        }

        void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            _serviceClient.Unsubscribe();
        }



        private void btnTrackStart_Click(object sender, EventArgs e)
        {
            if (SelectedContext != null)
            {
                _serviceClient.TrackingStart(SelectedContext);
            }
        }

        private void btnTrackStop_Click(object sender, EventArgs e)
        {
            _serviceClient.TrackingStop();
        }


        void PredictionCallCompleted(IAsyncResult ar)
        {
            var predictionCall = (Func<IEnumerable<ContextPreference>>)ar.AsyncState;

            var preferences = predictionCall.EndInvoke(ar);

            SendOrPostCallback setList = delegate
            {
                lstPreferences.Items.Clear();

                if (preferences != null)
                {
                    foreach (var preference in preferences.OrderByDescending(p => p.Value))
                    {

                        var lvi = new ListViewItem();
                        var contextName = _contextService.GetById(preference.ContextId.ToString()).ContextName;
                        lvi.Text = preference.ContextId.ToString();
                        lvi.SubItems.Add(contextName);
                        lvi.SubItems.Add(preference.Value.ToString());
                        lstPreferences.Items.Add(lvi);
                    }
                }
            };

            _syncContext.Post(setList, null);
        }

       
        
        private void btnPrediction_Click(object sender, EventArgs e)
        {
            try
            {
                progbarLocalize.Value = 0;
                btnPrediction.Enabled = false;
                lstPreferences.Items.Clear();
                var predictionCall = new Func<IEnumerable<ContextPreference>>(_serviceClient.GetCloserContexts);

                predictionCall.BeginInvoke(
                    PredictionCallCompleted,
                    predictionCall);
            }
            finally
            {
                // TODO..
                btnPrediction.Enabled = true;
            }
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void ReportServiceStatus(ServiceStatus serviceStatus)
        {
            Debug.WriteLine("[CLIENT] STATUS: " + serviceStatus.ServiceState.ToString());

            _logFile.WriteLine("{0} - report status: {1}, {2}",
                DateTime.Now.ToString(), 
                serviceStatus.ServiceState.ToString(), 
                serviceStatus.ContextId);
        }


        public void ReportLocalizationProgress(double progress)
        {
            SendOrPostCallback setProgressBar = delegate
            {
                int value = (int)(progress * progbarLocalize.Maximum + progbarLocalize.Minimum);
                progbarLocalize.Value = value;
                if (value > 0)
                {
                    progbarLocalize.Value = value - 1;
                }
                if(value == progbarLocalize.Maximum)
                {
                    progbarLocalize.Value = progbarLocalize.Maximum;
                }
            };

            _syncContext.Post(setProgressBar, null);
        }

        private void lstRecent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstRecent.SelectedItems.Count > 0)
            {
                SelectedContext = lstRecent.SelectedItems[0].Text;
                txtSelected.Text = lstRecent.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void lstPreferences_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPreferences.SelectedItems.Count > 0)
            {
                SelectedContext = lstPreferences.SelectedItems[0].Text;
                txtSelected.Text = lstPreferences.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadRecentContexts(5);
        }

        private void LoadRecentContexts(int limit)
        {
            lstRecent.Items.Clear();
            var contexts = _contextService.GetMostRecent(limit);
            foreach(var context in contexts)
            {
                var lvi = new ListViewItem();
                lvi.Text = context.Id.ToString();
                lvi.SubItems.Add(context.ContextName);
                lvi.SubItems.Add(context.CreationDateTime.ToString());
                lstRecent.Items.Add(lvi);
            }
        }

        public void ReportPrediction(string contextId)
        {
            Debug.WriteLine("[CLIENT] PREDICTION: " + contextId);
            _logFile.WriteLine("{0} - report prediction: {1}",
                DateTime.Now.ToString(),
                contextId);
        }

        private void btnSilence_Click(object sender, EventArgs e)
        {
            _serviceClient.Silence();
        }
    }
}
