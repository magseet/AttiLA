using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AttiLA.LocalizationService
{
    [ServiceContract]
    public interface ILocalizationService
    {
        [OperationContract]
        GlobalSettings GetGlobalSettings();

        [OperationContract]
        void SetGlobalSettings(GlobalSettings newSettings);
            
        [OperationContract]
        void ChangeContext(string newContextId);

        [OperationContract]
        void TrackModeStart();

        [OperationContract]
        void TrackModeStop();
        
    }

    [DataContract]
    public class GlobalSettings
    {
        [DataMember]
        TrackerSettings Tracker { get; set; }
    }


    /// <summary>
    /// Settings related to tracking module.
    /// </summary>
    [DataContract]
    public class TrackerSettings
    {
        /// <summary>
        /// Milliseconds between samples.
        /// </summary>
        [DataMember]
        public double Interval { get; set; }

        /// <summary>
        /// Milliseconds between savings.
        /// When the timeout fires, all tracked data are stored in database.
        /// </summary>
        public double UpdateInterval { get; set; }
    }


    [DataContract]
    public class AccessPoint
    {
        [DataMember]
        public string MAC { get; set; }

        [DataMember]
        public string SSID { get; set; }

        [DataMember]
        public int RSSI { get; set; }

        [DataMember]
        public uint LinkQuality { get; set; }
    }
}
