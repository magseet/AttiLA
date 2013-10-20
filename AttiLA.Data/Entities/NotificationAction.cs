using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class NotificationAction : Action
    {
        /// <summary>
        /// The message of the notification.
        /// </summary>
        public string Message { get; set; }

    }
}
