using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data.Entities
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// The result of a WLAN scan.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class TrainingSetExample
    {
        /// <summary>
        /// The time of the WLAN scan.
        /// </summary>
        public DateTime ScanDateTime { get; set; }

        /// <summary>
        /// The list of the access point signals.
        /// </summary>
        public List<ScanSignal> ScanSignals { get; set; }

        public TrainingSetExample()
        {
            ScanSignals = new List<ScanSignal>();
        }
    }
}
