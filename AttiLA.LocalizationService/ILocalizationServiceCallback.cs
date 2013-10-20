using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AttiLA.LocalizationService
{
    /// <summary>
    /// The callback contract for the localization service.
    /// </summary>
    public interface ILocalizationServiceCallback
    {
        /// <summary>
        /// Notification callback informing about progress of localization.
        /// </summary>
        /// <param name="progress">Progress value between 0.0 and 1.0</param>
        [OperationContract(IsOneWay = true)]
        void ReportLocalizationProgress(double progress);

        /// <summary>
        /// Notification callback informing about predicted context.
        /// </summary>
        /// <param name="contextId">The predicted context id.</param>
        [OperationContract(IsOneWay = true)]
        void ReportPrediction(string contextId);

        /// <summary>
        /// Notification callback informing about the service status.
        /// </summary>
        /// <param name="serviceStatus"></param>
        [OperationContract(IsOneWay = true)]
        void ReportServiceStatus(ServiceStatus serviceStatus);
    }
}
