using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Scenario : MongoEntity
    {
        /// <summary>
        /// The context ID associated to this scenario.
        /// </summary>
        public ObjectId ContextId { get; set; }

        /// <summary>
        /// The creation time.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// The last time the classifier was trained.
        /// </summary>
        public DateTime TrainingTime { get; set; }

        /// <summary>
        /// The last time new examples were added to the training set
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// The examples used to train the classifier for this scenario.
        /// </summary>
        public List<TrainingSetExample> TrainingSet { get; set; }

        /// <summary>
        /// The result of statistical computation on the training examples.
        /// </summary>
        [BsonIgnore]
        public List<Feature> Features { get; set; }

        /// <summary>
        /// Number of times the selection prediction was right.
        /// </summary>
        public long TruePositives { get; set; }

        /// <summary>
        /// Number of times the selection prediction was wrong.
        /// </summary>
        public long FalsePositives { get; set; }

        public Scenario()
        {
            Features = new List<Feature>();
            TrainingSet = new List<TrainingSetExample>();
        }
    }
}
