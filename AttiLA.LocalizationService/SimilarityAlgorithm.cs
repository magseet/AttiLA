using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AttiLA.Data.Entities;

namespace AttiLA.LocalizationService
{
    public class SimilarityAlgorithm
    {

        public SimilarityAlgorithm() { }

        public Func<IEnumerable<Feature>, IDictionary<AccessPoint, int>, double> this[SimilarityAlgorithmType type]
        {


            get
            {
                switch (type)
                {
                    case SimilarityAlgorithmType.NaiveBayes:
                        return delegate(IEnumerable<Feature> feature, IDictionary<AccessPoint, int> evindences)
                        {
                            return 0.0;
                        };


                    case SimilarityAlgorithmType.RelativeError:

                    case SimilarityAlgorithmType.RelativeError2:
                        break;


                }
                return null;
            }

        }
    }
}
