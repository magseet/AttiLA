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
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool EnabledOnStartupField;
        
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
        public bool EnabledOnStartup {
            get {
                return this.EnabledOnStartupField;
            }
            set {
                if ((this.EnabledOnStartupField.Equals(value) != true)) {
                    this.EnabledOnStartupField = value;
                    this.RaisePropertyChanged("EnabledOnStartup");
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
        [System.ServiceModel.FaultContractAttribute(typeof(System.ArgumentException), Action="http://tempuri.org/ILocalizationService/ChangeContextArgumentExceptionFault", Name="ArgumentException", Namespace="http://schemas.datacontract.org/2004/07/System")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/ChangeContextServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        void ChangeContext(string newContextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/ChangeContext", ReplyAction="http://tempuri.org/ILocalizationService/ChangeContextResponse")]
        System.Threading.Tasks.Task ChangeContextAsync(string newContextId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Localize", ReplyAction="http://tempuri.org/ILocalizationService/LocalizeResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(AttiLA.Test.LocalizationService.LocalizationServiceReference.ServiceException), Action="http://tempuri.org/ILocalizationService/LocalizeServiceExceptionFault", Name="ServiceException", Namespace="http://schemas.datacontract.org/2004/07/AttiLA.LocalizationService")]
        AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeResponse Localize(AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeRequest request);
        
        // CODEGEN: Verrà generato un contratto di messaggio perché l'operazione presenta più valori restituiti.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/Localize", ReplyAction="http://tempuri.org/ILocalizationService/LocalizeResponse")]
        System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeResponse> LocalizeAsync(AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStartResponse")]
        void TrackModeStart();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStart", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStartResponse")]
        System.Threading.Tasks.Task TrackModeStartAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStop", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStopResponse")]
        void TrackModeStop();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILocalizationService/TrackModeStop", ReplyAction="http://tempuri.org/ILocalizationService/TrackModeStopResponse")]
        System.Threading.Tasks.Task TrackModeStopAsync();
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Localize", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class LocalizeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool changeContext;
        
        public LocalizeRequest() {
        }
        
        public LocalizeRequest(bool changeContext) {
            this.changeContext = changeContext;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LocalizeResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class LocalizeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string LocalizeResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public AttiLA.Test.LocalizationService.LocalizationServiceReference.ContextPreference[] preferences;
        
        public LocalizeResponse() {
        }
        
        public LocalizeResponse(string LocalizeResult, AttiLA.Test.LocalizationService.LocalizationServiceReference.ContextPreference[] preferences) {
            this.LocalizeResult = LocalizeResult;
            this.preferences = preferences;
        }
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
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeResponse AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService.Localize(AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeRequest request) {
            return base.Channel.Localize(request);
        }
        
        public string Localize(bool changeContext, out AttiLA.Test.LocalizationService.LocalizationServiceReference.ContextPreference[] preferences) {
            AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeRequest inValue = new AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeRequest();
            inValue.changeContext = changeContext;
            AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeResponse retVal = ((AttiLA.Test.LocalizationService.LocalizationServiceReference.ILocalizationService)(this)).Localize(inValue);
            preferences = retVal.preferences;
            return retVal.LocalizeResult;
        }
        
        public System.Threading.Tasks.Task<AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeResponse> LocalizeAsync(AttiLA.Test.LocalizationService.LocalizationServiceReference.LocalizeRequest request) {
            return base.Channel.LocalizeAsync(request);
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
