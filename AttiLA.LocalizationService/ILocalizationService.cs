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
        [FaultContract(typeof(ServiceException))]
        void SetGlobalSettings(GlobalSettings newSettings);
            
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ArgumentException))]
        void ChangeContext(string newContextId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TrackModeStart();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TrackModeStop();
        
    }

    [DataContract]
    public class ContextSimilarity
    {
        [DataMember]
        string ContextId { get; set; }

        [DataMember]
        double Similarity { get; set; }
    }

    [DataContract]
    public class GlobalSettings
    {
        [DataMember]
        public TrackerSettings Tracking { get; set; }
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
        public double CaptureInterval { get; set; }

        /// <summary>
        /// Milliseconds between savings.
        /// When the timeout fires, all tracked data are stored in database.
        /// </summary>
        public double UpdateInterval { get; set; }
    }

    [DataContract]
    public class ServiceException
    {
        [DataMember]
        public string Message { get; set; }
    }
}
