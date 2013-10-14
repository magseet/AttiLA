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
            ContextSimilarity[] similarContexts;
            string contextId = serviceClient.Localize(true, out similarContexts);

            listViewContexts.Items.Clear();

            if(similarContexts != null)
            {
                foreach(var context in similarContexts)
                {
                    var lvi = new ListViewItem();
                    lvi.Text = context.ContextId;
                    lvi.SubItems.Add(context.Similarity.ToString());
                    listViewContexts.Items.Add(lvi);
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
