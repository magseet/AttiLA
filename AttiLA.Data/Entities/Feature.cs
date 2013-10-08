using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{
    [BsonIgnoreExtraElements]
    class Feature : MongoEntity
    {
        /// <summary>
        /// The number of samples used to calculate the current Gaussian parameters.
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// The Gaussian mean.
        /// </summary>
        public double Mu { get; set; }

        /// <summary>
        /// The Gaussian variance.
        /// </summary>
        public double Sigma2 { get; set; }

        /// <summary>
        /// The number of sample used to calculate the current Gaussian parameters for score.
        /// </summary>
        public int ScoreN { get; set; }

        /// <summary>
        /// The Gaussian mean for score.
        /// </summary>
        public double ScoreMu { get; set; }

        /// <summary>
        /// The Gaussian variance for score.
        /// </summary>
        public double ScoreSigma2 { get; set; }
    }
}
