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

namespace AttiLA.Test.LocalizationService
{
    [CallbackBehavior(
        //ConcurrencyMode = ConcurrencyMode.Reentrant
        UseSynchronizationContext=false
        )]
    public partial class Form1 : Form, ILocalizationServiceCallback
    {
        
        private LocalizationServiceClient serviceClient;

        private ContextService contextService = new ContextService();

        SynchronizationContext _SyncContext = null;

        private string SelectedContext { get; set; }
        
        public Form1()
        {
            InitializeComponent();
            _SyncContext = WindowsFormsSynchronizationContext.Current;
            var context = new InstanceContext(this);
            serviceClient = new LocalizationServiceClient(context);
            serviceClient.Subscribe();
        }

        void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            serviceClient.Unsubscribe();
        }



        private void btnChange_Click(object sender, EventArgs e)
        {
            /*
            if(!String.IsNullOrWhiteSpace(SelectedContext))
            {
                try
                {
                    serviceClient.ChangeContext(SelectedContext);
                }
                catch(FaultException<ArgumentException>)
                {
                    MessageBox.Show("invalid preference");
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
             */
            
        }

        private void btnTrackStart_Click(object sender, EventArgs e)
        {
            //serviceClient.TrackModeStart();
        }

        private void btnTrackStop_Click(object sender, EventArgs e)
        {
            //serviceClient.TrackModeStop();
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
                        var contextName = contextService.GetById(preference.ContextId.ToString()).ContextName;
                        lvi.Text = preference.ContextId.ToString();
                        lvi.SubItems.Add(contextName);
                        lvi.SubItems.Add(preference.Value.ToString());
                        lstPreferences.Items.Add(lvi);
                    }
                }
            };

            _SyncContext.Post(setList, null);
        }

       
        public delegate TV MyFunc<in T, TU, out TV>(T input, out TU output);
        private void btnPrediction_Click(object sender, EventArgs e)
        {
            try
            {
                progbarLocalize.Value = 0;
                btnPrediction.Enabled = false;
                lstPreferences.Items.Clear();
                var predictionCall = new Func<IEnumerable<ContextPreference>>(serviceClient.Prediction);

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

        public void TrackModeStarted(DateTime startTime, string contextId)
        {
            MessageBox.Show("Started!");
        }

        public void TrackModeStopped(DateTime stopTime, string contextId)
        {

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

            _SyncContext.Post(setProgressBar, null);
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
            var contexts = contextService.GetMoreRecent(limit);
            foreach(var context in contexts)
            {
                var lvi = new ListViewItem();
                lvi.Text = context.Id.ToString();
                lvi.SubItems.Add(context.ContextName);
                lvi.SubItems.Add(context.CreationDateTime.ToString());
                lstRecent.Items.Add(lvi);
            }
        }

    }
}
