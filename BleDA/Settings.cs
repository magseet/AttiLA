using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BleDA.LocalizationService;
using System.ServiceModel;

namespace BleDA
{
    #region Exceptions

    public enum SettingsExceptionCode
    {
        /// <summary>
        /// The service did not respond as expected
        /// </summary>
        ServiceFailure
    }

    public class SettingsException : Exception
    {
        public SettingsExceptionCode Code { get; set; }

        public SettingsException() : base() { }
        public SettingsException(string message) : base(message) { }
        public SettingsException(string message, System.Exception inner) : base(message, inner) { }
    }

    #endregion


    public class Settings : ILocalizationServiceCallback
    {

        #region Static members

        /// <summary>
        /// Static inizialization of the private instance.
        /// </summary>
        private static readonly Settings _instance = new Settings();

        #endregion

        private object lockSettings = new Object();
        private LocalizationServiceClient _serviceClient;

        /// <summary>
        /// Singleton class.
        /// </summary>
        private Settings() 
        {
            var context = new InstanceContext(this);
            _serviceClient = new LocalizationServiceClient(context);
        }

        /// <summary>
        /// Returns the unique instance of the singleton class.
        /// </summary>
        public static Settings Instance
        {
            get { return _instance; }
        }

        public GlobalSettings Service
        {
            get
            {
                lock (lockSettings)
                {
                    GlobalSettings settings = null;
                    for (var attempts = Properties.Settings.Default.ClientRetries + 1;
                            attempts > 0; attempts--)
                    {
                        try
                        {
                            settings = _serviceClient.GetGlobalSettings();
                            if(settings != null)
                            {
                                break;
                            }
                        }
                        catch { }

                    }
                    return null;
                }
            }
            set
            {
                lock (lockSettings)
                {
                    if(value == null)
                    {
                        throw new ArgumentNullException("value");
                    }
                    bool appliedSettings = false;
                    for (var attempts = Properties.Settings.Default.ClientRetries + 1;
                            attempts > 0; attempts--)
                    {
                        try
                        {
                            appliedSettings = _serviceClient.SetGlobalSettings(value);
                            if(appliedSettings)
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (attempts == 1)
                            {
                                throw new SettingsException(Properties.Resources.MsgApplySettingsFailure, ex);
                            }
                        }
                    }
                    if(!appliedSettings)
                    {
                        throw new SettingsException(Properties.Resources.MsgApplySettingsFailure);
                    }
                }
            }
        }

        #region Unused callbacks
        public void ReportLocalizationProgress(double progress)
        {
            //unused
            throw new NotImplementedException();
        }

        public void ReportPrediction(string contextId)
        {
            //unused
            throw new NotImplementedException();
        }

        public void ReportServiceStatus(ServiceStatus serviceStatus)
        {
            //unused
            throw new NotImplementedException();
        }
        #endregion
    }
}
