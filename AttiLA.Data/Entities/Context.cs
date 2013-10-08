using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    class Context : MongoEntity
    {
        public Context()
        {
            Features = new Dictionary<string, Feature>();
            Statistics = new List<ContextStatistics>();
        }

        /// <summary>
        /// The name assigned by the user to the context.
        /// </summary>
        public string ContextName { get; set; }

        /// <summary>
        /// The creation time.
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Data containing informations used to predict the actual context given the current signal relevations.
        /// </summary>
        public Dictionary<string, Feature> Features { get; set; }

        /// <summary>
        /// Statistics about context usage periods.
        /// </summary>
        public List<ContextStatistics> Statistics { get; set; }
    }
}
