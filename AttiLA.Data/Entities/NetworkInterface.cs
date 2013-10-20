using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    public enum NetworkInterfaceActionType
    {
        Connect,
        Disconnect,
        Nothing
    }

    [BsonIgnoreExtraElements]
    public class NetworkInterface
    {
        /// <summary>
        /// Interface identifier.
        /// </summary>
        public string InterfaceGuid { get; set; }

        /// <summary>
        /// Action to be performed on the interface.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public NetworkInterfaceActionType Action { get; set; }

        /// <summary>
        /// Network profile to be used in case of connection.
        /// </summary>
        public string ProfileName { get; set; }
    }
}
