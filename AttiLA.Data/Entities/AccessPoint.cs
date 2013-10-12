﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class AccessPoint
    {
        /// <summary>
        /// The MAC address of the access point.
        /// </summary>
        public string MAC { get; set; }

        /// <summary>
        /// The SSID of the network.
        /// </summary>
        public string SSID { get; set; }
    }
}
