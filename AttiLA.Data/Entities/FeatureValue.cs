using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class FeatureValue
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// The signal average.
        /// </summary>
        public double Avg { get; set; }

        /// <summary>
        /// The signal variance.
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// The percentage of times the signal was sampled.
        /// </summary>
        public double Reliability { get; set; }
    }
}
