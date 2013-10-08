using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    class ContextStatistics : MongoEntity
    {
        /// <summary>
        /// Usage period starting time.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Usage period leaving time.
        /// </summary>
        public DateTime LeaveDateTime { get; set; }
    }
}
