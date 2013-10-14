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
        /// Notification callback informing that the tracker has been started.
        /// </summary>
        /// <param name="startTime"></param>
        [OperationContract(IsOneWay = true)]
        void TrackModeStarted(DateTime startTime);


        /// <summary>
        /// Notification callback informing that the tracker has been stopped.
        /// </summary>
        /// <param name="stopTime"></param>
        [OperationContract(IsOneWay = true)]
        void TrackModeStopped(DateTime stopTime);
    }
}
