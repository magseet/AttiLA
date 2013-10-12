using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace AttiLA.Data.Entities
{
    public class Scenario : MongoEntity
    {
        /// <summary>
        /// The context ID associated to this scenario.
        /// </summary>
        public ObjectId ContextId { get; set; }

        /// <summary>
        /// The creation time.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// The last time the classifier war trained.
        /// </summary>
        public DateTime TrainingTime { get; set; }

        /// <summary>
        /// The examples used to train the classifier for this scenario.
        /// </summary>
        public List<ScanExample> TrainingSet { get; set; }

        /// <summary>
        /// The features used to classify a new example.
        /// </summary>
        public Dictionary<string, Feature> Features { get; set; }

        /// <summary>
        /// Number of times the selection prediction was right.
        /// </summary>
        public long TruePositives { get; set; }

        /// <summary>
        /// Number of times the selection prediction was wrong.
        /// </summary>
        public long FalsePositives { get; set; }

        public Scenario()
        {
            Features = new Dictionary<string, Feature>();
            TrainingSet = new List<ScanExample>();
        }
    }
}
