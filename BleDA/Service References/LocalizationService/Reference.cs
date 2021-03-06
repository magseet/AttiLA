﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.33440
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BleDA.LocalizationService {
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
        private BleDA.LocalizationService.ServiceStateCode ServiceStateField;
        
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
        public BleDA.LocalizationService.ServiceStateCode ServiceState {
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
        Notification = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Tracking = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Training = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GlobalSettings", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class GlobalSettings : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BleDA.LocalizationService.LocalizerSettings LocalizerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private uint NotificationThresholdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BleDA.LocalizationService.TrackerSettings TrackerField;
        
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
        public BleDA.LocalizationService.LocalizerSettings Localizer {
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
        public BleDA.LocalizationService.TrackerSettings Tracker {
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
        private double IntervalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private uint RetriesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BleDA.LocalizationService.SimilarityAlgorithmCode SimilarityAlgorithmField;
        
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
        public double Interval {
            get {
                return this.IntervalField;
            }
            set {
                if ((this.IntervalField.Equals(value) != true)) {
                    this.IntervalField = value;
                    this.RaisePropertyChanged("Interval");
                }
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
        public BleDA.LocalizationService.SimilarityAlgorithmCode SimilarityAlgorithm {
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
        private double IntervalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private uint TrainingThresholdField;
        
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
        public double Interval {
            get {
                return this.IntervalField;
            }
            set {
                if ((this.IntervalField.Equals(value) != true)) {
                    this.IntervalField = value;
                    this.RaisePropertyChanged("Interval");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public uint TrainingThreshold {
            get {
                return this.TrainingThresholdField;
            }
            set {
                if ((this.TrainingThresholdField.Equals(value) != true)) {
                    this.TrainingThresholdField = value;
                    this.RaisePropertyChanged("TrainingThreshold");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LocalizationService.ILocalizationService", CallbackContract=typeof(BleDA.LocalizationService.ILocalizationServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
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
        BleDA.LocalizationService.ServiceStatus GetServiceStatus();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetServiceStatus", ReplyAction="http://tempuri.org/ILocalizationService/GetServiceStatusResponse")]
        System.Threading.Tasks.Task<BleDA.LocalizationService.ServiceStatus> GetServiceStatusAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/GetGlobalSettingsResponse")]
        BleDA.LocalizationService.GlobalSettings GetGlobalSettings();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/GetGlobalSettingsResponse")]
        System.Threading.Tasks.Task<BleDA.LocalizationService.GlobalSettings> GetGlobalSettingsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/SetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/SetGlobalSettingsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(BleDA.LocalizationService.ServiceException), Action="http://tempuri.org/ILocalizationService/SetGlobalSettingsServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        bool SetGlobalSettings(BleDA.LocalizationService.GlobalSettings newSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/SetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/SetGlobalSettingsResponse")]
        System.Threading.Tasks.Task<bool> SetGlobalSettingsAsync(BleDA.LocalizationService.GlobalSettings newSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetCloserContexts", ReplyAction="http://tempuri.org/ILocalizationService/GetCloserContextsResponse")]
        BleDA.LocalizationService.ContextPreference[] GetCloserContexts();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetCloserContexts", ReplyAction="http://tempuri.org/ILocalizationService/GetCloserContextsResponse")]
        System.Threading.Tasks.Task<BleDA.LocalizationService.ContextPreference[]> GetCloserContextsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackingStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackingStartResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(BleDA.LocalizationService.ServiceException), Action="http://tempuri.org/ILocalizationService/TrackingStartServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        bool TrackingStart(string contextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackingStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackingStartResponse")]
        System.Threading.Tasks.Task<bool> TrackingStartAsync(string contextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackingStop", ReplyAction="http://tempuri.org/ILocalizationService/TrackingStopResponse")]
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
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ILocalizationService/ReportLocalizationProgress")]
        void ReportLocalizationProgress(double progress);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ILocalizationService/ReportPrediction")]
        void ReportPrediction(string contextId);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ILocalizationService/ReportServiceStatus")]
        void ReportServiceStatus(BleDA.LocalizationService.ServiceStatus serviceStatus);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILocalizationServiceChannel : BleDA.LocalizationService.ILocalizationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LocalizationServiceClient : System.ServiceModel.DuplexClientBase<BleDA.LocalizationService.ILocalizationService>, BleDA.LocalizationService.ILocalizationService {
        
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
        
        public BleDA.LocalizationService.ServiceStatus GetServiceStatus() {
            return base.Channel.GetServiceStatus();
        }
        
        public System.Threading.Tasks.Task<BleDA.LocalizationService.ServiceStatus> GetServiceStatusAsync() {
            return base.Channel.GetServiceStatusAsync();
        }
        
        public BleDA.LocalizationService.GlobalSettings GetGlobalSettings() {
            return base.Channel.GetGlobalSettings();
        }
        
        public System.Threading.Tasks.Task<BleDA.LocalizationService.GlobalSettings> GetGlobalSettingsAsync() {
            return base.Channel.GetGlobalSettingsAsync();
        }
        
        public bool SetGlobalSettings(BleDA.LocalizationService.GlobalSettings newSettings) {
            return base.Channel.SetGlobalSettings(newSettings);
        }
        
        public System.Threading.Tasks.Task<bool> SetGlobalSettingsAsync(BleDA.LocalizationService.GlobalSettings newSettings) {
            return base.Channel.SetGlobalSettingsAsync(newSettings);
        }
        
        public BleDA.LocalizationService.ContextPreference[] GetCloserContexts() {
            return base.Channel.GetCloserContexts();
        }
        
        public System.Threading.Tasks.Task<BleDA.LocalizationService.ContextPreference[]> GetCloserContextsAsync() {
            return base.Channel.GetCloserContextsAsync();
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
