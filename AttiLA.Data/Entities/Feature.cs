using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Feature : MongoEntity
    {
        
        /// <summary>
        /// The Gaussian mean.
        /// </summary>
        public double Mean { get; set; }

        /// <summary>
        /// The Gaussian variance.
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// The percentage of variance explained by the feature.
        /// </summary>
        public int Expl { get; set; }
    }
}
