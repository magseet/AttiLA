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
    public class ScanExample : MongoEntity
    {
        /// <summary>
        /// The time of the WLAN scan.
        /// </summary>
        public DateTime ScanDateTime { get; set; }

        /// <summary>
        /// The signals strenghts mapped on MAC addresses.
        /// </summary>
        public Dictionary<string, uint> ScanSignals { get; set; }

        public ScanExample()
        {
            ScanSignals = new Dictionary<string, uint>();
        }


    }
}
