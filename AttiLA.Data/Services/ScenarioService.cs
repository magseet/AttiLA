using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AttiLA.Data.Services
{
    public class ScenarioService : EntityService<Scenario>
    {

        /// <summary>
        /// Adds new scan examples to the training set.
        /// </summary>
        /// <exception cref="DataServiceExceptions">Thrown when the new example is not inserted due to an error.</exception>
        /// <param name="scenarioId"></param>
        /// <param name="scanExamples">The list of examples to add</param>
        public void AddScanExamples(string scenarioId, IEnumerable<TrainingSetExample> scanExamples)
        {
            if (scanExamples == null)
            {
                throw new ArgumentNullException("scanExamples");
            }
            var scenarioObjectId = new ObjectId(scenarioId);

            WriteConcernResult updateResult;
            try
            {
                //// Append the new example to the scenario training set
                updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                    Query<Scenario>.EQ(scenario => scenario.Id, scenarioObjectId),
                    Update<Scenario>.PushAll(scenario => scenario.TrainingSet, scanExamples),
                    new MongoUpdateOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });
            }
            catch(MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorAddScanExample, ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorAddScanExample);
            }
        }

        /// <summary>
        /// Returns all the scenarios containing at least one of the specified access points.
        /// </summary>
        /// <param name="accessPoints">The MAC addresses associated to the possible accessPoints.</param>
        /// <returns></returns>
        public IEnumerable<Scenario> GetByPossibleAccessPoints(IEnumerable<AccessPoint> accessPoints)
        {
            if (accessPoints == null)
            {
                throw new ArgumentNullException("accessPoints");
            }
            if(accessPoints.Count() == 0)
            {
                return new List<Scenario>().AsEnumerable();
            }

            //// Get all ids of scenarios containing at least one access point in the list
            var accessPointsBSON = new BsonArray();
            foreach (var ap in accessPoints)
            {
                accessPointsBSON.Add(ap.ToBsonDocument());
            }

            // Produce a document for each example in the training set
            var unwindTrainingSet = new BsonDocument
            {
              { "$unwind", "$" + Utils<Scenario>.MemberName(s => s.TrainingSet) } 
            };

            // Produce a document for each signal in the example
            var unwindScanSignals = new BsonDocument
            {
                { 
                    "$unwind", 
                    "$" + Utils<Scenario>.MemberName(s => s.TrainingSet) +
                    "." + Utils<TrainingSetExample>.MemberName(e => e.ScanSignals) 
                }
            };

            // show access point on top
            var projectAccessPoint = new BsonDocument
            {
                { 
                    "$project",
                    new BsonDocument
                    {
                        { Utils<Scenario>.MemberName(s => s.ContextId), 1 },
                        { Utils<Scenario>.MemberName(s => s.CreationTime), 1 },
                        { Utils<Scenario>.MemberName(s => s.TrainingTime), 1 },
                        { 
                            Utils<Scenario>.MemberName(s => s.UpdateTime),
                            "$" + Utils<Scenario>.MemberName(s => s.TrainingSet) +
                            "." + Utils<TrainingSetExample>.MemberName(e => e.ScanDateTime)
                        },
                        { Utils<Scenario>.MemberName(s => s.Features), 1 },
                        { Utils<Scenario>.MemberName(s => s.TruePositives), 1 },
                        { Utils<Scenario>.MemberName(s => s.FalsePositives), 1 },
                        {
                            Utils<ScanSignal>.MemberName(s => s.AP),
                            "$" + Utils<Scenario>.MemberName(s => s.TrainingSet) +
                            "." + Utils<TrainingSetExample>.MemberName(e => e.ScanSignals) +
                            "." + Utils<ScanSignal>.MemberName(s => s.AP)
                        }
                    }
                }
            };

            // Filter out document using access point list
            var matchAccessPoints = new BsonDocument
            {
                {
                    "$match",
                    new BsonDocument
                    {
                        {
                            Utils<ScanSignal>.MemberName(s => s.AP),
                            new BsonDocument
                            {
                                { "$in", accessPointsBSON }
                            }
                        }
                    }
                }
            };

            // access point no more needed, group using all other fields
            var group = new BsonDocument
            {
                {
                    "$group",
                    new BsonDocument
                    {
                        { "_id", "$_id" },
                        { 
                            Utils<Scenario>.MemberName(s => s.ContextId),
                            new BsonDocument
                            {
                                { "$first", "$" + Utils<Scenario>.MemberName(s => s.ContextId) }
                            }
                        },
                        { 
                            Utils<Scenario>.MemberName(s => s.CreationTime),
                            new BsonDocument
                            {
                                { "$first", "$" + Utils<Scenario>.MemberName(s => s.CreationTime) }
                            }
                        },
                        { 
                            Utils<Scenario>.MemberName(s => s.TrainingTime),
                            new BsonDocument
                            {
                                { "$first", "$" + Utils<Scenario>.MemberName(s => s.TrainingTime) }
                            }
                        },
                        { 
                            Utils<Scenario>.MemberName(s => s.UpdateTime),
                            new BsonDocument
                            {
                                { "$max", "$" + Utils<Scenario>.MemberName(s => s.UpdateTime) }
                            }
                        },
                        { 
                            Utils<Scenario>.MemberName(s => s.Features),
                            new BsonDocument
                            {
                                { "$first", "$" + Utils<Scenario>.MemberName(s => s.Features) }
                            }
                        },
                        { 
                            Utils<Scenario>.MemberName(s => s.TruePositives),
                            new BsonDocument
                            {
                                { "$first", "$" + Utils<Scenario>.MemberName(s => s.TruePositives) }
                            }
                        },
                        { 
                            Utils<Scenario>.MemberName(s => s.FalsePositives),
                            new BsonDocument
                            {
                                { "$first", "$" + Utils<Scenario>.MemberName(s => s.FalsePositives) }
                            }
                        }
                    }
                }
            };


            // prepare aggregation pipeline
            var pipeline = new[] 
            {
                unwindTrainingSet, 
                unwindScanSignals,
                projectAccessPoint,
                matchAccessPoints,
                group
            };

            List<Scenario> scenarios;
            try
            {
                var result = this.MongoConnectionHandler.MongoCollection.Aggregate(pipeline);
                scenarios = result.ResultDocuments
                    .Select(BsonSerializer.Deserialize<Scenario>)
                    .ToList();
            }
            catch(MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorScenarioRead, ex);
            }

            if(scenarios.Count == 0)
            {
                // no suitable scenarios found
                return scenarios.AsEnumerable();
            }



            // prepare for map reduce
            string ts = Utils<Scenario>.MemberName(s => s.TrainingSet);
            string signals = Utils<TrainingSetExample>.MemberName(e => e.ScanSignals);
            string signalAP = Utils<ScanSignal>.MemberName(s => s.AP);
            string featureAP = Utils<FeatureKey>.MemberName(f => f.AP);
            string signalValue = Utils<ScanSignal>.MemberName(s => s.RSSI);
            string scenarioId = Utils<FeatureKey>.MemberName(f => f.ScenarioId);
            string map = @"
                function map() {
	                var ts_length = this." + ts + @".length;
	                var scenario_id = this._id;
	                this." + ts + @".forEach(function(ts) {
		                ts." + signals + @".forEach(function(signal) {
			                emit(
                            {
                                " + scenarioId + @": scenario_id,
                                " + featureAP + @": signal." + signalAP + @"
                            },
			                {	
				                sum: signal." + signalValue + @",
				                min: signal." + signalValue + @",
				                max: signal." + signalValue + @",
				                count: 1,
				                diff: 0,
				                count_max : ts_length
    		                });
		                });
	                });
                }";

            string reduce = @"
                function reduce(key, values) {
                    var a = values[0]; // will reduce into here
                    for (var i=1/*!*/; i < values.length; i++){
                        var b = values[i]; // will merge 'b' into 'a'

                        // temp helpers
                        var delta = a.sum/a.count - b.sum/b.count; // a.mean - b.mean
                        var weight = (a.count * b.count)/(a.count + b.count);
        
                        // do the reducing
                        a.diff += b.diff + delta*delta*weight;
                        a.sum += b.sum;
                        a.count += b.count;
                        a.min = Math.min(a.min, b.min);
                        a.max = Math.max(a.max, b.max);
                    }
                    return a;
                }";

            string avg = Utils<FeatureValue>.MemberName(f => f.Avg);
            string variance = Utils<FeatureValue>.MemberName(f => f.Variance);
            string min = Utils<FeatureValue>.MemberName(f => f.Min);
            string max = Utils<FeatureValue>.MemberName(f => f.Max);
            string reliability = Utils<FeatureValue>.MemberName(f => f.Reliability);

            string finalize=@"
                function finalize(key, value) { 
	                var res = new Object();
                    res." + avg + @" = value.sum / value.count;
                    res." + variance + @" = value.diff / value.count;
                    res." + reliability + @" = value.count / value.count_max;
	                res." + min + @" = value.min;
	                res." + max + @" = value.max;
                    return res;
                }";

            var options = MapReduceOptions
                .SetOutput(MapReduceOutput.Merge(Utils<Feature>.CollectionName))
                .SetFinalize(new BsonJavaScript(finalize));


            var featureService = new FeatureService();

            foreach (var scenario in scenarios)
            {
                if(scenario.UpdateTime > scenario.TrainingTime)
                {
                    // perform training
                    var query = Query<Scenario>.EQ(s => s.Id, scenario.Id);
                    try
                    {
                        var result = this.MongoConnectionHandler.MongoCollection
                            .MapReduce(query, map, reduce, options);
                        scenario.Features = result.GetResults().Select(BsonSerializer.Deserialize<Feature>).ToList();
                        var update = Update<Scenario>.Set(s => s.TrainingTime, DateTime.Now);
                        this.MongoConnectionHandler.MongoCollection.FindAndModify(query, SortBy.Null, update);
                        
                    }
                    catch (MongoException ex)
                    {
                        throw new DatabaseException(Properties.Resources.MsgErrorScenarioTrain + scenario.Id, ex);
                    }

                }
                else
                {
                    // training not needed
                    scenario.Features = featureService.GetByScenario(scenario.Id.ToString()).ToList();
                    
                    
                }
            }

            return scenarios.AsEnumerable();
        }

        public void prova()
        {

            var accessPoints = new List<AccessPoint>
            {
                new AccessPoint{MAC = "00146C58355E", SSID = "NETGEAR"},
                new AccessPoint{MAC = "4494FC43DA9E", SSID = "StudioL"},
                new AccessPoint{MAC = "F07D686BC630", SSID = "D-Link"},
            };

            GetByPossibleAccessPoints(accessPoints);
        }

        public override void Update(Scenario entity)
        {
            throw new NotImplementedException();
        }
    }
}
