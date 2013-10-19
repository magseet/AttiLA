using System;
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
using AttiLA.Data;
using NativeWifi;
using BleDA.ActionService;


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
            //prova();
        }

       

        void prova()
        {
            var processes = Processes.GetUserProcesses();
            foreach(var process in processes)
            {
                Console.WriteLine((string)process.ManagementObject["CommandLine"]);
            }


            WlanClient client = new WlanClient();
            foreach(var wlanIface in client.Interfaces)
            {
                //wlanIface.Connect(Wlan.WlanConnectionMode.Auto,Wlan.Dot11BssType.Any)
                var profiles = wlanIface.GetProfiles();
                foreach(var profile in profiles)
                {
                    var profileXml = wlanIface.GetProfileXml(profile.profileName);
                    MessageBox.Show(profileXml);
                    
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Context c = new Context
            {
                ContextName = this.ContextName,
                CreationDateTime = DateTime.Now
            };
            try
            {
                contextService.Create(c);
            }
            catch(DatabaseException)
            {
                MessageBox.Show("Already exists.");
                return;
            }

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
