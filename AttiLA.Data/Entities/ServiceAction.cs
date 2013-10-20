using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    public enum ServiceActionType
    {
        Stop,
        Start,
        Restart
    }

    [BsonIgnoreExtraElements]
    public class ServiceAction : Action
    {
        /// <summary>
        /// The name of the Windows Service.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The type of action on the service.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public ServiceActionType ServiceActionType { get; set; }

    }
}
