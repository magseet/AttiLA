using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    /// <summary>
    /// This class is the result of a map reduce in database to calculate
    /// statistic values on data.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class Feature : IMongoEntity
    {
        /// <summary>
        /// unused id
        /// </summary>
        [BsonIgnore]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Feature identification.
        /// </summary>
        [BsonId]
        public FeatureKey Key { get; set; }

        /// <summary>
        /// Feature value.
        /// </summary>
        [BsonElement("value")]
        public FeatureValue Value { get; set; }
    }
}
