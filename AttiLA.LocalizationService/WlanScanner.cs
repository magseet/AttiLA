using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeWifi;
using System.Threading;

namespace AttiLA.LocalizationService
{
    /// <summary>
    /// Performs interaction with Managed Wifi service.
    /// </summary>
    public class WlanScanner
    {

        /// <summary>
        /// Managed Wifi object
        /// </summary>
        private WlanClient wlanClient = new WlanClient();

        /// <summary>
        /// For each interface scan complete events are monitored.
        /// </summary>
        private Dictionary<Guid, AutoResetEvent> wlanEventScanComplete = new Dictionary<Guid, AutoResetEvent>();


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
                Wlan.WlanNotificationCodeMsm notificationCode = (Wlan.WlanNotificationCodeMsm)notifyData.notificationCode;
            }
            else if (notifyData.notificationSource == Wlan.WlanNotificationSource.ACM)
            {
                Wlan.WlanNotificationCodeAcm notificationCode = (Wlan.WlanNotificationCodeAcm)notifyData.notificationCode;
                switch (notificationCode)
                {
                    case Wlan.WlanNotificationCodeAcm.ScanComplete:
                        // signal scan completion by the interface
                        wlanEventScanComplete[notifyData.interfaceGuid].Set();
                        break;
                }
            }

        }



        public WlanScanner()
        {
            foreach (WlanClient.WlanInterface wlanIface in wlanClient.Interfaces)
            {
                // create events for the interface
                wlanEventScanComplete.Add(wlanIface.InterfaceGuid, new AutoResetEvent(false));
                // set wlan notifications handler
                wlanIface.WlanNotification += wlanIface_WlanNotification;
            }
        }
    }
}
