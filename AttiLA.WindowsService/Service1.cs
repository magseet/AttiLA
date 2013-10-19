using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace AttiLA.WindowsService
{
    /// <summary>
    /// The Windows Service class hosting the WCF localization service.
    /// </summary>
    public partial class AttiLAWindowsService : ServiceBase
    {
        /// <summary>
        /// The hosted WCF service.
        /// </summary>
        private ServiceHost _serviceHost;

        public AttiLAWindowsService()
        {
            InitializeComponent();

#if ! DEBUG
            if (!System.Diagnostics.EventLog.SourceExists("AttiLAservice"))
            {
                System.Diagnostics.EventLog.CreateEventSource("AttiLAservice", "AttiLAlog");
            }
            eventLog.Source = "AttiLAservice";
            eventLog.Log = "AttiLAlog"; 
#endif
        }

#if DEBUG      
        public void OnDebug()
        {
            OnStart(null);
        }
#endif

        protected override void OnStart(string[] args)
        {
#if ! DEBUG
            eventLog.WriteEntry(Properties.Resources.LogMsgServiceStarted);
#endif
            if(_serviceHost != null)
            {
                _serviceHost.Close();
            }
            // Create a URI to serve as the base address
            Uri baseAddr = new Uri(Properties.Settings.Default.ServiceHostURI);
            
            // Create ServiceHost
            _serviceHost = new ServiceHost(
                typeof(AttiLA.LocalizationService.LocalizationService), 
                baseAddr);

            try
            {
                // Add net named pipe service endpoint

                //var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                var binding = new NetNamedPipeBinding();
#if DEBUG
                binding.CloseTimeout = TimeSpan.FromMinutes(1.0);
                binding.OpenTimeout = TimeSpan.FromMinutes(1.0);
                binding.ReceiveTimeout = TimeSpan.FromMinutes(30.0);
                binding.SendTimeout = TimeSpan.FromMinutes(30.0);
#endif
                
                binding.Name = Properties.Settings.Default.BindingPipeName;
                _serviceHost.AddServiceEndpoint(
                    typeof(AttiLA.LocalizationService.ILocalizationService), 
                    binding,
                    Properties.Settings.Default.LocalizationServicePipe);

                // Enable metadata exchange
                ServiceMetadataBehavior smb = _serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                {
                    smb = new ServiceMetadataBehavior();
                    _serviceHost.Description.Behaviors.Add(smb);
                }
                
                // Add MEX endpoint
                _serviceHost.AddServiceEndpoint(
                    ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexNamedPipeBinding(),
                    Properties.Settings.Default.MessageServicePipe);

                // Start the Service
                _serviceHost.Open();

            }
            catch(CommunicationException)
            {
                _serviceHost.Abort();
                Thread.CurrentThread.Abort();
            }
        }

        protected override void OnStop()
        {
#if ! DEBUG
            eventLog.WriteEntry(Properties.Resources.LogMsgServiceStopped);
#endif
            if(_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
        }
    }
}
