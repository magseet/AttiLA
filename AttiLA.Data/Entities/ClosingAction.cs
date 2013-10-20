using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class ClosingAction : Action
    {
        /// <summary>
        /// The process name.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// The path of the executable file starting the process.
        /// </summary>
        public string ExecutablePath { get; set; }

        /// <summary>
        /// The complete command used to start the process.
        /// </summary>
        public string CommandLine { get; set; }
    }
}
