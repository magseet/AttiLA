using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AttiLA.LocalizationService
{

    /// <summary>
    /// WCF service that performs localization based on WLAN signals.
    /// </summary>
    [ServiceContract(
        SessionMode=SessionMode.Required, 
        CallbackContract=typeof(ILocalizationServiceCallback))]
    public interface ILocalizationService
    {
        /// <summary>
        /// Operation of subscription to the callback service.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool Subscribe();


        /// <summary>
        /// Operation of cancellation from the callback service.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool Unsubscribe();

        /// <summary>
        /// Obtain informations about service settings.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GlobalSettings GetGlobalSettings();


        /// <summary>
        /// Change service settings.
        /// </summary>
        /// <param name="newSettings"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SetGlobalSettings(GlobalSettings newSettings);
            

        /// <summary>
        /// Operation used to inform the service that a context has been
        /// manually selected.
        /// </summary>
        /// <param name="newContextId">The manually selected context id.</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [FaultContract(typeof(ArgumentException))]
        void ChangeContext(string newContextId);


        /// <summary>
        /// Operation providing a prediction of the most suitable context id 
        /// for the current position. It is possible to switch immediately to 
        /// the predicted context. Other suitable contexts are provided with 
        /// their similarity2 value.
        /// </summary>
        /// <param name="changeContext">Switch immediately to the context or not.</param>
        /// <param name="similarContexts">Suitable contexts with similarity2 value.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Localize(bool changeContext, out IEnumerable<ContextSimilarity> similarContexts);

        /// <summary>
        /// Operation to enable the tracker.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TrackModeStart();

        /// <summary>
        /// Operation to disable the tracker.
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TrackModeStop();
        
    }

    /// <summary>
    /// Informations about a context similarity2 detected on prediction.
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
        /// The context value of similarity2 to the predicted one.
        /// </summary>
        [DataMember]
        public double Similarity { get; set; }
    }

    /// <summary>
    /// The system global settings.
    /// </summary>
    [DataContract]
    public class GlobalSettings
    {
        /// <summary>
        /// The tracking section.
        /// </summary>
        [DataMember]
        public TrackerSettings Tracking { get; set; }

        public LocalizationSettings Localization { get; set; }
        
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
        [DataMember]
        public double UpdateInterval { get; set; }

        /// <summary>
        /// Flag to enable the tracker on service startup (localize and track).
        /// </summary>
        [DataMember]
        public bool EnabledOnStartup { get; set; }
    }


    /// <summary>
    /// Settings related to localize module.
    /// </summary>
    [DataContract]
    public class LocalizationSettings
    {
        [DataMember]
        SimilarityAlgorithmType SimilarityAlgorithm { get; set; }
    }

    /// <summary>
    /// Information about a system operation failure.
    /// </summary>
    [DataContract]
    public class ServiceException
    {
        /// <summary>
        /// Message explaining the failure.
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }

    /// <summary>
    /// Values for similarity algorithm option.
    /// </summary>
    [DataContract]
    public enum SimilarityAlgorithmType
    {
        [EnumMember]
        NaiveBayes,
        [EnumMember]
        RelativeError,
        [EnumMember]
        RelativeError2
    }
}
