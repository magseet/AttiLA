using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AttiLA.LocalizationService
{
    /// <summary>
    /// The subscriber contract for the localization service.
    /// </summary>
    public interface ILocalizationServiceCallback
    {
        /// <summary>
        /// Notification subscriber informing that the tracker has been started.
        /// </summary>
        /// <param name="startTime"></param>
        [OperationContract(IsOneWay = true)]
        void TrackModeStarted(DateTime startTime, string contextId);


        /// <summary>
        /// Notification subscriber informing that the tracker has been stopped.
        /// </summary>
        /// <param name="stopTime"></param>
        [OperationContract(IsOneWay = true)]
        void TrackModeStopped(DateTime stopTime, string contextId);


        /// <summary>
        /// Notification subscriber informing about progress of localization.
        /// </summary>
        /// <param name="progress">Progress value between 0.0 and 1.0</param>
        [OperationContract(IsOneWay = true)]
        void ReportLocalizationProgress(double progress);
    }
}
