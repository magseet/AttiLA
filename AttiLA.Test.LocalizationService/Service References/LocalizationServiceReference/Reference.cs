﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.18213
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AttiLA.Test.LocalizationService.LocalizationServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceStatus", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class ServiceStatus : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContextIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceStateCode ServiceStateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ContextId {
            get {
                return this.ContextIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ContextIdField, value) != true)) {
                    this.ContextIdField = value;
                    this.RaisePropertyChanged("ContextId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceStateCode ServiceState {
            get {
                return this.ServiceStateField;
            }
            set {
                if ((this.ServiceStateField.Equals(value) != true)) {
                    this.ServiceStateField = value;
                    this.RaisePropertyChanged("ServiceState");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceStateCode", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    public enum ServiceStateCode : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Idle = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Tracking = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Notification = 2,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GlobalSettings", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class GlobalSettings : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizerSettings LocalizerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private uint NotificationThresholdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double PredictionIntervalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AttiLA.Test.LocalizationService.LocalizationServiceReference.TrackerSettings TrackerField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizerSettings Localizer {
            get {
                return this.LocalizerField;
            }
            set {
                if ((object.ReferenceEquals(this.LocalizerField, value) != true)) {
                    this.LocalizerField = value;
                    this.RaisePropertyChanged("Localizer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public uint NotificationThreshold {
            get {
                return this.NotificationThresholdField;
            }
            set {
                if ((this.NotificationThresholdField.Equals(value) != true)) {
                    this.NotificationThresholdField = value;
                    this.RaisePropertyChanged("NotificationThreshold");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double PredictionInterval {
            get {
                return this.PredictionIntervalField;
            }
            set {
                if ((this.PredictionIntervalField.Equals(value) != true)) {
                    this.PredictionIntervalField = value;
                    this.RaisePropertyChanged("PredictionInterval");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.TrackerSettings Tracker {
            get {
                return this.TrackerField;
            }
            set {
                if ((object.ReferenceEquals(this.TrackerField, value) != true)) {
                    this.TrackerField = value;
                    this.RaisePropertyChanged("Tracker");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LocalizerSettings", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class LocalizerSettings : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private uint RetriesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AttiLA.Test.LocalizationService.LocalizationServiceReference.SimilarityAlgorithmCode SimilarityAlgorithmField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public uint Retries {
            get {
                return this.RetriesField;
            }
            set {
                if ((this.RetriesField.Equals(value) != true)) {
                    this.RetriesField = value;
                    this.RaisePropertyChanged("Retries");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.SimilarityAlgorithmCode SimilarityAlgorithm {
            get {
                return this.SimilarityAlgorithmField;
            }
            set {
                if ((this.SimilarityAlgorithmField.Equals(value) != true)) {
                    this.SimilarityAlgorithmField = value;
                    this.RaisePropertyChanged("SimilarityAlgorithm");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TrackerSettings", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class TrackerSettings : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double CaptureIntervalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double UpdateIntervalField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double CaptureInterval {
            get {
                return this.CaptureIntervalField;
            }
            set {
                if ((this.CaptureIntervalField.Equals(value) != true)) {
                    this.CaptureIntervalField = value;
                    this.RaisePropertyChanged("CaptureInterval");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double UpdateInterval {
            get {
                return this.UpdateIntervalField;
            }
            set {
                if ((this.UpdateIntervalField.Equals(value) != true)) {
                    this.UpdateIntervalField = value;
                    this.RaisePropertyChanged("UpdateInterval");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SimilarityAlgorithmCode", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    public enum SimilarityAlgorithmCode : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NaiveBayes = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RelativeErrorExtended = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RelativeError = 2,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class ServiceException : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ContextPreference", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class ContextPreference : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContextIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ContextId {
            get {
                return this.ContextIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ContextIdField, value) != true)) {
                    this.ContextIdField = value;
                    this.RaisePropertyChanged("ContextId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Value {
            get {
                return this.ValueField;
            }
            set {
                if ((this.ValueField.Equals(value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LocalizationServiceReference.ILocalizationService", CallbackContract=typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface ILocalizationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Subscribe", ReplyAction="http://tempuri.org/ILocalizationService/SubscribeResponse")]
        bool Subscribe();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Subscribe", ReplyAction="http://tempuri.org/ILocalizationService/SubscribeResponse")]
        System.Threading.Tasks.Task<bool> SubscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Unsubscribe", ReplyAction="http://tempuri.org/ILocalizationService/UnsubscribeResponse")]
        bool Unsubscribe();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Unsubscribe", ReplyAction="http://tempuri.org/ILocalizationService/UnsubscribeResponse")]
        System.Threading.Tasks.Task<bool> UnsubscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetServiceStatus", ReplyAction="http://tempuri.org/ILocalizationService/GetServiceStatusResponse")]
        AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceStatus GetServiceStatus();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetServiceStatus", ReplyAction="http://tempuri.org/ILocalizationService/GetServiceStatusResponse")]
        System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceStatus> GetServiceStatusAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/GetGlobalSettingsResponse")]
        AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings GetGlobalSettings();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/GetGlobalSettingsResponse")]
        System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings> GetGlobalSettingsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/SetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/SetGlobalSettingsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/SetGlobalSettingsServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        bool SetGlobalSettings(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/SetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/SetGlobalSettingsResponse")]
        System.Threading.Tasks.Task<bool> SetGlobalSettingsAsync(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Prediction", ReplyAction="http://tempuri.org/ILocalizationService/PredictionResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/PredictionServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        AttiLA.Test.LocalizationService.LocalizationServiceReference.ContextPreference[] Prediction();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Prediction", ReplyAction="http://tempuri.org/ILocalizationService/PredictionResponse")]
        System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.ContextPreference[]> PredictionAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackingStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackingStartResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/TrackingStartServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        bool TrackingStart(string contextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackingStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackingStartResponse")]
        System.Threading.Tasks.Task<bool> TrackingStartAsync(string contextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackingStop", ReplyAction="http://tempuri.org/ILocalizationService/TrackingStopResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/TrackingStopServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        bool TrackingStop();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackingStop", ReplyAction="http://tempuri.org/ILocalizationService/TrackingStopResponse")]
        System.Threading.Tasks.Task<bool> TrackingStopAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Silence", ReplyAction="http://tempuri.org/ILocalizationService/SilenceResponse")]
        void Silence();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Silence", ReplyAction="http://tempuri.org/ILocalizationService/SilenceResponse")]
        System.Threading.Tasks.Task SilenceAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILocalizationServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ILocalizationService/TrackModeStarted")]
        void TrackModeStarted(System.DateTime startTime, string contextId);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ILocalizationService/TrackModeStopped")]
        void TrackModeStopped(System.DateTime stopTime, string contextId);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ILocalizationService/ReportLocalizationProgress")]
        void ReportLocalizationProgress(double progress);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILocalizationServiceChannel : AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LocalizationServiceClient : System.ServiceModel.DuplexClientBase<AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService>, AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService {
        
        public LocalizationServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public LocalizationServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public LocalizationServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public LocalizationServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public LocalizationServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool Subscribe() {
            return base.Channel.Subscribe();
        }
        
        public System.Threading.Tasks.Task<bool> SubscribeAsync() {
            return base.Channel.SubscribeAsync();
        }
        
        public bool Unsubscribe() {
            return base.Channel.Unsubscribe();
        }
        
        public System.Threading.Tasks.Task<bool> UnsubscribeAsync() {
            return base.Channel.UnsubscribeAsync();
        }
        
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceStatus GetServiceStatus() {
            return base.Channel.GetServiceStatus();
        }
        
        public System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceStatus> GetServiceStatusAsync() {
            return base.Channel.GetServiceStatusAsync();
        }
        
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings GetGlobalSettings() {
            return base.Channel.GetGlobalSettings();
        }
        
        public System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings> GetGlobalSettingsAsync() {
            return base.Channel.GetGlobalSettingsAsync();
        }
        
        public bool SetGlobalSettings(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings) {
            return base.Channel.SetGlobalSettings(newSettings);
        }
        
        public System.Threading.Tasks.Task<bool> SetGlobalSettingsAsync(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings) {
            return base.Channel.SetGlobalSettingsAsync(newSettings);
        }
        
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.ContextPreference[] Prediction() {
            return base.Channel.Prediction();
        }
        
        public System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.ContextPreference[]> PredictionAsync() {
            return base.Channel.PredictionAsync();
        }
        
        public bool TrackingStart(string contextId) {
            return base.Channel.TrackingStart(contextId);
        }
        
        public System.Threading.Tasks.Task<bool> TrackingStartAsync(string contextId) {
            return base.Channel.TrackingStartAsync(contextId);
        }
        
        public bool TrackingStop() {
            return base.Channel.TrackingStop();
        }
        
        public System.Threading.Tasks.Task<bool> TrackingStopAsync() {
            return base.Channel.TrackingStopAsync();
        }
        
        public void Silence() {
            base.Channel.Silence();
        }
        
        public System.Threading.Tasks.Task SilenceAsync() {
            return base.Channel.SilenceAsync();
        }
    }
}
