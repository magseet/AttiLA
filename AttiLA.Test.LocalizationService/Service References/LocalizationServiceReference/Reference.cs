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
    [System.Runtime.Serialization.DataContractAttribute(Name="GlobalSettings", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
    [System.SerializableAttribute()]
    public partial class GlobalSettings : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AttiLA.Test.LocalizationService.LocalizationServiceReference.TrackerSettings TrackingField;
        
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
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.TrackerSettings Tracking {
            get {
                return this.TrackingField;
            }
            set {
                if ((object.ReferenceEquals(this.TrackingField, value) != true)) {
                    this.TrackingField = value;
                    this.RaisePropertyChanged("Tracking");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LocalizationServiceReference.ILocalizationService")]
    public interface ILocalizationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/GetGlobalSettingsResponse")]
        AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings GetGlobalSettings();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/GetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/GetGlobalSettingsResponse")]
        System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings> GetGlobalSettingsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/SetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/SetGlobalSettingsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/SetGlobalSettingsServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        void SetGlobalSettings(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/SetGlobalSettings", ReplyAction="http://tempuri.org/ILocalizationService/SetGlobalSettingsResponse")]
        System.Threading.Tasks.Task SetGlobalSettingsAsync(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/ChangeContext", ReplyAction="http://tempuri.org/ILocalizationService/ChangeContextResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/ChangeContextServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        [System.ServiceModel.FaultContractAttribute(typeof(System.ArgumentException), Action="http://tempuri.org/ILocalizationService/ChangeContextArgumentExceptionFault", Name="ArgumentException", Namespace="http://schemas.datacontract.org/2004/07/System")]
        void ChangeContext(string newContextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/ChangeContext", ReplyAction="http://tempuri.org/ILocalizationService/ChangeContextResponse")]
        System.Threading.Tasks.Task ChangeContextAsync(string newContextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStartResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/TrackModeStartServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        void TrackModeStart();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStartResponse")]
        System.Threading.Tasks.Task TrackModeStartAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStop", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStopResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/TrackModeStopServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        void TrackModeStop();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStop", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStopResponse")]
        System.Threading.Tasks.Task TrackModeStopAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILocalizationServiceChannel : AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LocalizationServiceClient : System.ServiceModel.ClientBase<AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService>, AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService {
        
        public LocalizationServiceClient() {
        }
        
        public LocalizationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LocalizationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LocalizationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LocalizationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings GetGlobalSettings() {
            return base.Channel.GetGlobalSettings();
        }
        
        public System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings> GetGlobalSettingsAsync() {
            return base.Channel.GetGlobalSettingsAsync();
        }
        
        public void SetGlobalSettings(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings) {
            base.Channel.SetGlobalSettings(newSettings);
        }
        
        public System.Threading.Tasks.Task SetGlobalSettingsAsync(AttiLA.Test.LocalizationService.LocalizationServiceReference.GlobalSettings newSettings) {
            return base.Channel.SetGlobalSettingsAsync(newSettings);
        }
        
        public void ChangeContext(string newContextId) {
            base.Channel.ChangeContext(newContextId);
        }
        
        public System.Threading.Tasks.Task ChangeContextAsync(string newContextId) {
            return base.Channel.ChangeContextAsync(newContextId);
        }
        
        public void TrackModeStart() {
            base.Channel.TrackModeStart();
        }
        
        public System.Threading.Tasks.Task TrackModeStartAsync() {
            return base.Channel.TrackModeStartAsync();
        }
        
        public void TrackModeStop() {
            base.Channel.TrackModeStop();
        }
        
        public System.Threading.Tasks.Task TrackModeStopAsync() {
            return base.Channel.TrackModeStopAsync();
        }
    }
}
