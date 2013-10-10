using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using System.ServiceModel;
using System.ServiceModel.Description;
using NativeWifi;
using System.Threading;
using AttiLA.Data;

namespace AttiLA.WindowsService
{
    public partial class AttiLAWindowsService : ServiceBase
    {
        ServiceHost serviceHost;

        /// <summary>
        /// Manager Wifi object
        /// </summary>
        private WlanClient wlanClient;
        private Dictionary<Guid, AutoResetEvent> wlanEventScanComplete;

        int skipScan = 0;
        /// <summary>
        /// Function that scans WLAN interfaces and returns the signals read.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, uint> getSignals()
        {
            Dictionary<string, uint> signals = new Dictionary<string, uint>();
            foreach (WlanClient.WlanInterface wlanIface in wlanClient.Interfaces)
            {
                if (skipScan == 0)
                {
                    // scan for new network and wait for notification
                    wlanIface.Scan();
                    wlanEventScanComplete[wlanIface.InterfaceGuid].WaitOne();
                }
                skipScan = (skipScan + 1) % 10;
                
                Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                foreach (Wlan.WlanBssEntry network in wlanBssEntries)
                {
                    byte[] macAddr = network.dot11Bssid;
                    string tMac = "";
                    for (int i = 0; i < macAddr.Length; i++)
                    {
                        tMac += macAddr[i].ToString("x2").PadLeft(2, '0').ToUpper();
                    }

                    signals.Add(tMac, network.linkQuality);
                }
            }
            return signals;
        }

        /// <summary>
        /// WLAN notification handler
        /// </summary>
        /// <param name="notifyData"></param>
        void wlanIface_WlanNotification(Wlan.WlanNotificationData notifyData)
        {
            if (notifyData.notificationSource == Wlan.WlanNotificationSource.MSM)
            {
                Wlan.WlanNotificationCodeMsm notificationCode = (Wlan.WlanNotificationCodeMsm) notifyData.notificationCode;
            }
			else if (notifyData.notificationSource == Wlan.WlanNotificationSource.ACM)
            {
                Wlan.WlanNotificationCodeAcm notificationCode = (Wlan.WlanNotificationCodeAcm) notifyData.notificationCode;
                switch(notificationCode)
                {
                    case Wlan.WlanNotificationCodeAcm.ScanComplete:
                        // signal scan completion by the interface
                        wlanEventScanComplete[notifyData.interfaceGuid].Set();
                        break;
                }
            }

        }


        public AttiLAWindowsService()
        {
            InitializeComponent();
            // initialize managed wifi
            wlanClient = new WlanClient();
            // initialize wlan events
            wlanEventScanComplete = new Dictionary<Guid, AutoResetEvent>();

            foreach (WlanClient.WlanInterface wlanIface in wlanClient.Interfaces)
            {
                // create events for the interface
                wlanEventScanComplete.Add(wlanIface.InterfaceGuid, new AutoResetEvent(false));
                // set wlan notifications handler
                wlanIface.WlanNotification += wlanIface_WlanNotification;
            }


#if ! DEBUG
            if (!System.Diagnostics.EventLog.SourceExists("AttiLAservice"))
            {
                System.Diagnostics.EventLog.CreateEventSource("AttiLAservice", "AttiLAlog");
            }
            eventLog.Source = "AttiLAservice";
            eventLog.Log = "AttiLAlog"; 
#endif
        }

        

        public void OnDebug()
        {
            OnStart(null);

            Context c = new Context();
            Dictionary<string, uint> signals = getSignals();
            Console.WriteLine("Score: {0}", c.Score(signals));
            c.Correct(signals);
            Console.WriteLine("Score: {0}", c.Score(signals));
            
            while(true)
            {
                signals = getSignals();
                Console.WriteLine("Signals: {0} - Score: {1}", signals.Count, c.Score(signals));
                c.Correct(signals);
                System.Threading.Thread.Sleep(300);
            }
        }
        protected override void OnStart(string[] args)
        {
#if ! DEBUG
            eventLog.WriteEntry(AttiLA.Properties.Resources.logmsg_start);
#endif
            if(serviceHost != null)
            {
                serviceHost.Close();
            }
            // Create a URI to serve as the base address
            Uri httpUrl = new Uri(Properties.Settings.Default.ServiceHostURL);
            // Create ServiceHost
            serviceHost = new ServiceHost(typeof(AttiLA.LocalizationService.LocalizationService), httpUrl);
            try
            {
                // Add a service endpoint
                serviceHost.AddServiceEndpoint
                (typeof(AttiLA.LocalizationService.ILocalizationService), new WSHttpBinding(), "");
                // Enable metadata exchange
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                serviceHost.Description.Behaviors.Add(smb);
                // Start the Service
                serviceHost.Open();

            }
            catch(CommunicationException)
            {
                serviceHost.Abort();
                Thread.CurrentThread.Abort();
            }
        }

        protected override void OnStop()
        {
#if ! DEBUG
            eventLog.WriteEntry(AttiLA.Properties.Resources.logmsg_stop);
#endif
            if(serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
