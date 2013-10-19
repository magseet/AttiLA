using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    public enum ActionType
    {
        Open,
        Close,
        Service,
        Notification
    }

    [BsonIgnoreExtraElements]
    public class Action
    {
        /// <summary>
        /// The action type.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public ActionType Type { get; set; }
    }
}
