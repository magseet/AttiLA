using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class OpeningAction : Action
    {
        /// <summary>
        /// Executable path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Arguments passed to executable on load.
        /// </summary>
        public string Arguments { get; set; }
    }
}
