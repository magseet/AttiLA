using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Task : MongoEntity
    {
        /// <summary>
        /// The name of the task.
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// The task creation time.
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// The list of actions to be executed in the specified order.
        /// </summary>
        public List<Action> Actions { get; set; }

        /// <summary>
        /// The list of contexts using the task.
        /// </summary>
        public List<ObjectId> Contexts { get; set; }

        public Task()
        {
            Actions = new List<Action>();
            Contexts = new List<ObjectId>();
        }
    }
}
