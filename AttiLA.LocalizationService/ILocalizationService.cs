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


        /// <summary>
        /// Service providing a prediction of the most suitable context id for 
        /// the current position. It is possible to switch immediately to the 
        /// predicted context. Othe suitable contexts are provided with their
        /// similarity value.
        /// </summary>
        /// <param name="changeContext">Switch immediately to the context or not.</param>
        /// <param name="similarContexts">Suitable contexts with similarity value.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Localize(bool changeContext, out IEnumerable<ContextSimilarity> similarContexts);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TrackModeStart();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TrackModeStop();
        
    }

    /// <summary>
    /// Information about a context similarity detected on prediction.
    /// </summary>
    [DataContract]
    public class ContextSimilarity
    {
        /// <summary>
        /// The context identifier.
        /// </summary>
        [DataMember]
        public string ContextId { get; set; }

        /// <summary>
        /// The context value of similarity to the predicted one.
        /// </summary>
        [DataMember]
        public double Similarity { get; set; }
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
