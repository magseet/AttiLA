using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data
{

    public class old_Context
    {
        private Dictionary<string, Feature> features;

        public old_Context()
        {
            features = new Dictionary<string, Feature>();
        }

        /// <summary>
        /// Update the scenario with the new signals.
        /// </summary>
        /// <param name="signals"></param>
        public void Correct(Dictionary<string,uint> signals)
        {
            Feature f;

            // update the scenario with the signal values
            foreach (KeyValuePair<string,uint> signal in signals)
            {
                if(!features.TryGetValue(signal.Key, out f))
                {
                    // add the new signal to the scenario
                    f = new Feature();
                    features.Add(signal.Key, f);
                }
                f.Hit(signal.Value/100.0);
            }

            // update missing signals of the scenario
            foreach (string missing in features.Keys.Except(signals.Keys))
            {
                features[missing].Miss();
            }
        }

        /// <summary>
        /// Returns the likelihood value between the signals and the scenario.
        /// </summary>
        /// <param name="signals"></param>
        /// <returns></returns>
        public double Score(Dictionary<string,uint> signals)
        {
            Feature f;
            double score = 0;
            double scoreMax = (signals.Keys.Union(features.Keys)).Count();
            foreach (KeyValuePair<string, uint> signal in signals)
            {
                if (features.TryGetValue(signal.Key, out f))
                {
                    double s = f.Score(signal.Value/100.0);
                    score += s * s;
                }
            }
            return Math.Sqrt(score);
        }

        private class Feature
        {
            private int N;
            private double mu;
            private double sigma2;
            private int score_N;
            private double score_mu;
            private double score_sigma2;

            internal double MinSigma2 { private get; set; }
            
            public Feature()
            {
                MinSigma2 = 0.2;
                mu = 0;
                sigma2 = MinSigma2;
                N = 0;
                score_sigma2 = MinSigma2;
                score_N = 0;
                score_mu = 0;
            }

            /// <summary>
            /// Update the feature using the value as a new sample.
            /// </summary>
            /// <param name="value"></param>
            public void Hit(double value)
            {
                double new_mu;

                // update mu sigma2
                if (N == 0)
                {
                    mu = value;
                    //sigma2 = value * value;
                }
                else
                {
                    new_mu = (N * mu + value) / (N + 1);
                    sigma2 = sigma2 * (N - 1) / N + Math.Pow(value - mu, 2) / (N + 1);
                    if (sigma2 < MinSigma2)
                    {
                        sigma2 = MinSigma2;
                    }
                    mu = new_mu;
                }
                N++;

                // make score better
                if (score_N == 0)
                {
                    score_mu = 1.0;
                    //score_sigma2 = 1.0;
                }
                else
                {
                    new_mu = (score_N * score_mu + 1.0) / (score_N + 1);
                    
                    // useless.. 
                    score_sigma2 = score_sigma2 * (score_N - 1) / score_N + Math.Pow(1.0 - score_mu, 2) / (score_N + 1);
                    
                    score_mu = new_mu;
                }
                score_N++;

            }

            /// <summary>
            /// Update the feature based on the fact that the sample is missing.
            /// </summary>
            public void Miss()
            {
                // make score worse
                if (score_N == 0)
                {
                    score_mu = 0.0;
                    //score_sigma2 = 1.0;
                }
                else
                {
                    double new_mu = (score_N * score_mu) / (score_N + 1);
                    score_sigma2 = score_sigma2 * (score_N - 1) / score_N + score_mu * score_mu / (score_N + 1);
                    score_mu = new_mu;
                }
                score_N++;
            }

            /// <summary>
            /// Returns a score for the passing value.
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public double Score(double value)
            {
                if (sigma2 == 0)
                {
                    return value == mu ? score_mu : 0;
                }
                double Z = (value - mu);
                Z = Z * Z;
                Z = -(Z / (2 * sigma2 * sigma2));
                return score_mu * Math.Exp(Z);
            }
        }

    }
}
