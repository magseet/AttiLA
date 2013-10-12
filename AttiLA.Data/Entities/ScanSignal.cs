using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class ScanSignal
    {
        /// <summary>
        /// Access point data.
        /// </summary>
        public AccessPoint AP { get; set; }

        /// <summary>
        /// Measurement of the power.
        /// Not serialized in the database.
        /// </summary>
        public int RSSI { get; set; }
    }
}
