using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Context : MongoEntity
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
        /// The examples used to train the classifier for this context.
        /// </summary>
        public List<ScanExample> TrainingSet { get; set; }

        /// <summary>
        /// The features used to classify a new example.
        /// </summary>
        public Dictionary<string, Feature> Features { get; set; }

        /// <summary>
        /// Statistics about context usage periods.
        /// </summary>
        public List<ContextStatistics> Statistics { get; set; }
    }
}
