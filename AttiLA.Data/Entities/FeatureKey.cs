using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class FeatureKey
    {
        /// <summary>
        /// Scenario identifier.
        /// </summary>
        public ObjectId ScenarioId { get; set; }

        /// <summary>
        /// Access point data.
        /// </summary>
        public AccessPoint AP { get; set; }
    }
}
