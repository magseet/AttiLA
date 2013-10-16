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
            
        }

        private void buttonTrackStart_Click(object sender, EventArgs e)
        {
            serviceClient.TrackModeStart();
        }

        private void buttonTrackStop_Click(object sender, EventArgs e)
        {
            serviceClient.TrackModeStop();
        }


        void LocalizeCallCompleted(IAsyncResult ar)
        {
            MessageBox.Show("Localization completed");
        }

       
        public delegate TV MyFunc<in T, TU, out TV>(T input, out TU output);
        private void buttonLocalize_Click(object sender, EventArgs e)
        {
             
            ContextPreference[] preferences;
            try
            {
                progressBarLocalize.Value = 0;
                buttonLocalize.Enabled = false;
                MyFunc<bool, ContextPreference[],string> localizeCall = new MyFunc<bool,ContextPreference[],string>(serviceClient.Localize);

                localizeCall.BeginInvoke(
                    true, 
                    out preferences,
                    LocalizeCallCompleted,
                    localizeCall);                
            }
            finally
            {
                // TODO..
                buttonLocalize.Enabled = true;
            }
            

            listViewContexts.Items.Clear();

            if(preferences != null)
            {
                foreach(var preference in preferences)
                {

                    var lvi = new ListViewItem();
                    var contextName = contextService.GetById(preference.ContextId.ToString()).ContextName;
                    lvi.Text = preference.ContextId.ToString();
                    lvi.SubItems.Add(contextName);
                    lvi.SubItems.Add(preference.Value.ToString());
                    listViewContexts.Items.Add(lvi);
                }
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
                progressBarLocalize.Value = (int)(progress * 100);
                label4.Text = (progress * 100).ToString();
            };

            _SyncContext.Post(setProgressBar, null);
        }
    }
}
