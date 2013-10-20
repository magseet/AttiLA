using System;
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

        #region Static members

        /// <summary>
        /// Static inizialization of the private instance.
        /// </summary>
        private static readonly WlanScanner _instance = new WlanScanner();

        #endregion

        #region Private members

        /// <summary>
        /// The lock used to synchronize access to the scanner.
        /// </summary>
        private Object _scannerLock = new Object();

        /// <summary>
        /// Managed Wifi object
        /// </summary>
        private WlanClient _wlanClient = new WlanClient();

        /// <summary>
        /// For each interface scan complete events are monitored.
        /// </summary>
        private Dictionary<Guid, AutoResetEvent> _wlanEventScanComplete = new Dictionary<Guid, AutoResetEvent>();

        private uint _retries = 3;

        #endregion

        #region Properties

        /// <summary>
        /// Number of times the service try to recover from a no signal scan.
        /// </summary>
        private uint Retries 
        { 
            get
            {
                lock(_scannerLock)
                {
                    return _retries;
                }
            }
            set
            {
                lock(_scannerLock)
                {
                    _retries = value;
                }
            }
        }

        #endregion

        /// <summary>
        /// Performs a scan of the WLAN interfaces and returns a list of <see cref="AccessPoint"/> elements.
        /// </summary>
        /// <returns></returns>
        public List<ScanSignal> GetScanSignals()
        {
            var scanSignals = new List<ScanSignal>();

            lock(_scannerLock)
            {
                foreach (WlanClient.WlanInterface wlanIface in _wlanClient.Interfaces)
                {
                    Wlan.WlanBssEntry[] wlanBssEntries = null;
                    for (var attempts = this.Retries + 1; attempts > 0; attempts--)
                    {
                        // scan for new bss and wait for notification
                        wlanIface.Scan();
                        _wlanEventScanComplete[wlanIface.InterfaceGuid].WaitOne();
                        wlanBssEntries = wlanIface.GetNetworkBssList();
                        
                        if (wlanBssEntries.Count() > 0)
                        {
                            break;
                        }
                    }

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

                        var signal = new ScanSignal
                        {
                            AP = new AccessPoint
                            {
                                MAC = tMac,
                                SSID = ssid
                            },
                            RSSI = bss.rssi,
                        };

                        scanSignals.Add(signal);
                    }
                }
            }
            return scanSignals;
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
                        _wlanEventScanComplete[notifyData.interfaceGuid].Set();
                        break;
                }
            }

        }

        private WlanScanner()
        {
            foreach (WlanClient.WlanInterface wlanIface in _wlanClient.Interfaces)
            {
                // create events for the interface
                _wlanEventScanComplete.Add(wlanIface.InterfaceGuid, new AutoResetEvent(false));
                // set wlan notifications handler
                wlanIface.WlanNotification += wlanIface_WlanNotification;
            }
        }

        /// <summary>
        /// Returns the unique instance of the singleton class.
        /// </summary>
        public static WlanScanner Instance
        {
            get { return _instance; }
        }
    }
}
