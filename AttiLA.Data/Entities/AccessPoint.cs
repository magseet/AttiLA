using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class AccessPoint : IEquatable<AccessPoint>
    {
        /// <summary>
        /// The MAC address of the access point.
        /// </summary>
        public string MAC { get; set; }

        /// <summary>
        /// The SSID of the network.
        /// </summary>
        public string SSID { get; set; }

        public bool Equals(AccessPoint other)
        {
            if(other == null)
            {
                return false;
            }
            return MAC == other.MAC && SSID == other.SSID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AccessPoint);
        }

        public override int GetHashCode()
        {
            return 17 * MAC.GetHashCode() + SSID.GetHashCode();
        }

    }
}
