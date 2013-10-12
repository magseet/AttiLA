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
        ServiceHost serviceHost;

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
            if(serviceHost != null)
            {
                serviceHost.Close();
            }
            // Create a URI to serve as the base address
            Uri baseAddr = new Uri(Properties.Settings.Default.ServiceHostURI);
            
            // Create ServiceHost
            serviceHost = new ServiceHost(
                typeof(AttiLA.LocalizationService.LocalizationService), 
                baseAddr);

            try
            {
                // Add net named pipe service endpoint

                //var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                var binding = new NetNamedPipeBinding();

                binding.Name = Properties.Settings.Default.BindingPipeName;
                serviceHost.AddServiceEndpoint(
                    typeof(AttiLA.LocalizationService.ILocalizationService), 
                    binding,
                    Properties.Settings.Default.LocalizationServicePipe);

                // Enable metadata exchange
                ServiceMetadataBehavior smb = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                {
                    smb = new ServiceMetadataBehavior();
                    serviceHost.Description.Behaviors.Add(smb);
                }
                
                // Add MEX endpoint
                serviceHost.AddServiceEndpoint(
                    ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexNamedPipeBinding(),
                    Properties.Settings.Default.MessageServicePipe);

                // Start the Service
                serviceHost.Open();

            }
            catch(CommunicationException)
            {
                serviceHost.Abort();
                Thread.CurrentThread.Abort();
            }
        }

        protected override void OnStop()
        {
#if ! DEBUG
            eventLog.WriteEntry(Properties.Resources.LogMsgServiceStopped);
#endif
            if(serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
