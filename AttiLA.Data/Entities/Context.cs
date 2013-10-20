using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Context : MongoEntity
    {
        /// <summary>
        /// The name assigned by the user to the scenario.
        /// </summary>
        public string ContextName { get; set; }

        /// <summary>
        /// The creation time.
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Actions on network interfaces.
        /// </summary>
        public List<NetworkInterface> NetworkInterfaces { get; set; }

        /// <summary>
        /// Statistics about scenario usage periods.
        /// </summary>
        public List<ContextStatistics> Statistics { get; set; }

        public Context()
        {
            Statistics = new List<ContextStatistics>();
            NetworkInterfaces = new List<NetworkInterface>();
        }
    }
}
