﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeWifi;
using System.Threading;
using AttiLA.Data.Entities;

namespace AttiLA.LocalizationService
{
    /// <summary>
    /// Performs interaction with Managed Wifi service.
    /// This class is thread safe and singleton.
    /// </summary>
    public sealed class WlanScanner
    {
        /// <summary>
        /// Static inizialization of the private instance.
        /// </summary>
        private static readonly WlanScanner instance = new WlanScanner();

        /// <summary>
        /// The lock used to synchronize access to the scanner.
        /// </summary>
        private Object scannerLock = new Object();

        /// <summary>
        /// Managed Wifi object
        /// </summary>
        private WlanClient wlanClient = new WlanClient();

        /// <summary>
        /// For each interface scan complete events are monitored.
        /// </summary>
        private Dictionary<Guid, AutoResetEvent> wlanEventScanComplete = new Dictionary<Guid, AutoResetEvent>();


        /// <summary>
        /// Performs a scan of the WLAN interfaces and returns a list of <see cref="AccessPoint"/> elements.
        /// </summary>
        /// <returns></returns>
        public List<AccessPoint> GetAccessPoints()
        {
            var accessPoints = new List<AccessPoint>();

            lock(scannerLock)
            {
                foreach (WlanClient.WlanInterface wlanIface in wlanClient.Interfaces)
                {
                    // scan for new bss and wait for notification
                    wlanIface.Scan();
                    wlanEventScanComplete[wlanIface.InterfaceGuid].WaitOne();

                    Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();

                    foreach (Wlan.WlanBssEntry bss in wlanBssEntries)
                    {
                        byte[] macAddr = bss.dot11Bssid;
                        string tMac = "";
                        for (int i = 0; i < macAddr.Length; i++)
                        {
                            tMac += macAddr[i].ToString("x2").PadLeft(2, '0').ToUpper();
                        }
                        string ssid = Encoding.ASCII.GetString(
                            bss.dot11Ssid.SSID, 
                            0, (int)bss.dot11Ssid.SSIDLength);

                        accessPoints.Add(new AccessPoint
                        {
                            MAC = tMac,
                            SSID = ssid,
                            RSSI = bss.rssi,
                        });
                    }
                }
            }
            return accessPoints;
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

        private WlanScanner()
        {
            foreach (WlanClient.WlanInterface wlanIface in wlanClient.Interfaces)
            {
                // create events for the interface
                wlanEventScanComplete.Add(wlanIface.InterfaceGuid, new AutoResetEvent(false));
                // set wlan notifications handler
                wlanIface.WlanNotification += wlanIface_WlanNotification;
            }
        }

        /// <summary>
        /// Returns the unique instance of the singleton class.
        /// </summary>
        public static WlanScanner Instance
        {
            get { return instance; }
        }
    }
}