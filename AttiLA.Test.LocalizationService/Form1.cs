using System;
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
using AttiLA.Data.Services;
using AttiLA.Data.Entities;


namespace AttiLA.Test.LocalizationService
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class Form1 : Form, ILocalizationServiceCallback
    {
        
        private LocalizationServiceClient serviceClient;

        private ContextService contextService = new ContextService();
        
        public Form1()
        {
            InitializeComponent();
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

        private void buttonLocalize_Click(object sender, EventArgs e)
        {
            ContextPreference[] preferences;
            try
            {
                buttonLocalize.Enabled = false;
                string contextId = serviceClient.Localize(true, out preferences);
            }
            finally
            {
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

        public void TrackModeStarted(DateTime startTime)
        {

        }

        public void TrackModeStopped(DateTime stopTime)
        {

        }
    }
}
