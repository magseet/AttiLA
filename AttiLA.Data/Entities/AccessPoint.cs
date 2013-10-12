using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class AccessPoint : MongoEntity
    {
        /// <summary>
        /// The MAC address of the access point.
        /// </summary>
        public string MAC { get; set; }

        /// <summary>
        /// The SSID of the network.
        /// </summary>
        public string SSID { get; set; }

        /// <summary>
        /// Measurement of the power.
        /// Not serialized in the database.
        /// </summary>
        [BsonIgnore]
        public int RSSI { get; set; }
    }
}
