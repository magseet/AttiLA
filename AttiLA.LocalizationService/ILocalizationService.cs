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
        /// Operation of subscription to the subscriber service.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool Subscribe();


        /// <summary>
        /// Operation of cancellation from the subscriber service.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool Unsubscribe();

        /// <summary>
        /// Obtain informations about service status.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ServiceStatus GetServiceStatus();

        /// <summary>
        /// Obtain informations about service settings.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GlobalSettings GetGlobalSettings();

        /// <summary>
        /// Change service settings. At the end of the operation, the service
        /// is in <see cref="Idle"/> state.
        /// </summary>
        /// <param name="newSettings"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SetGlobalSettings(GlobalSettings newSettings);
            

        /// <summary>
        /// Operation providing a prediction of the most suitable contexts 
        /// in the current position.
        /// </summary>
        /// <returns>
        /// Suitable contexts with preference value or null in case of error.
        /// </returns>
        [OperationContract]
        IEnumerable<ContextPreference> GetCloserContexts();

        /// <summary>
        /// Operation to put the service in <see cref="Tracking"/> state.
        /// </summary>
        /// <param name="contextId">The context to be tracked.</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool TrackingStart(string contextId);

        /// <summary>
        /// Operation to abandon the <see cref="Tracking"/> state.
        /// </summary>
        [OperationContract]
        bool TrackingStop();

        /// <summary>
        /// Operation to put the service in <see cref="Idle"/> state.
        /// </summary>
        [OperationContract]
        void Silence();

    }

    /// <summary>
    /// Context preference prediction.
    /// </summary>
    [DataContract]
    public class ContextPreference
    {
        /// <summary>
        /// The preference identifier.
        /// </summary>
        [DataMember]
        public string ContextId { get; set; }

        /// <summary>
        /// The preference value associated to the preference.
        /// </summary>
        [DataMember]
        public double Value { get; set; }
    }

    /// <summary>
    /// The global settings.
    /// </summary>
    [DataContract]
    public class GlobalSettings
    {
        /// <summary>
        /// The tracker settings.
        /// </summary>
        [DataMember]
        public TrackerSettings Tracker { get; set; }

        /// <summary>
        /// The localizer settings.
        /// </summary>
        [DataMember]
        public LocalizerSettings Localizer { get; set; }

        /// <summary>
        /// Number of correct predictions required for a notification.
        /// </summary>
        [DataMember]
        public uint NotificationThreshold { get; set; }
    }

    /// <summary>
    /// The service status.
    /// </summary>
    [DataContract]
    public class ServiceStatus
    {
        /// <summary>
        /// The service state.
        /// </summary>
        [DataMember]
        public ServiceStateCode ServiceState { get; set; }

        /// <summary>
        /// The target context id.
        /// </summary>
        [DataMember]
        public string ContextId { get; set; }
    }


    /// <summary>
    /// Settings related to tracking module.
    /// </summary>
    [DataContract]
    public class TrackerSettings
    {
        /// <summary>
        /// Minimum time in milliseconds between samples.
        /// </summary>
        [DataMember]
        public double Interval { get; set; }

        /// <summary>
        /// Number of samples required to train a scenario.
        /// </summary>
        [DataMember]
        public uint TrainingThreshold { get; set; }
    }


    /// <summary>
    /// Settings related to localizer module.
    /// </summary>
    [DataContract]
    public class LocalizerSettings
    {
        /// <summary>
        /// Minimum time in milliseconds between predictions.
        /// </summary>
        [DataMember]
        public double Interval { get; set; }

        /// <summary>
        /// Times the localizer try to recover from wrong predictions.
        /// </summary>
        [DataMember]
        public uint Retries { get; set; }

        /// <summary>
        /// The algorithm used to calculate signals affinity to a scenario.
        /// </summary>
        [DataMember]
        public SimilarityAlgorithmCode SimilarityAlgorithm { get; set; }
    }

    /// <summary>
    /// Information about a system operation failure.
    /// </summary>
    [DataContract]
    public class ServiceException
    {
        /// <summary>
        /// The service exception code.
        /// </summary>
        public ServiceExceptionCode Code { get; set; }

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
    public enum SimilarityAlgorithmCode
    {
        [EnumMember]
        NaiveBayes,
        [EnumMember]
        RelativeErrorExtended,
        [EnumMember]
        RelativeError
    }


    /// <summary>
    /// Values for service status option.
    /// </summary>
    [DataContract]
    public enum ServiceStateCode
    {
        /// <summary>
        /// All kind of training is disabled.
        /// </summary>
        [EnumMember]
        Idle,
        /// <summary>
        /// The service sends a notification when the context changes.
        /// </summary>
        [EnumMember]
        Notification,
        /// <summary>
        /// The service enters in training mode when the context changes.
        /// </summary>
        [EnumMember]
        Tracking,
        /// <summary>
        /// The service is in traning mode.
        /// </summary>
        [EnumMember]
        Training
    }

    /// <summary>
    /// Values for service exception.
    /// </summary>
    [DataContract]
    public enum ServiceExceptionCode
    {
        [EnumMember]
        Settings,
        [EnumMember]
        Arguments
    }
}
