using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AttiLA.Data.Services
{
    public class ScenarioService : EntityService<Scenario>
    {
        /// <summary>
        /// Adds a new scan example to the scenario.
        /// </summary>
        /// <exception cref="DataServiceExceptions">Thrown when the new example is not inserted due to an error.</exception>
        /// <param name="scenarioId"></param>
        /// <param name="scanExample"></param>
        public void AddScanExample(string scenarioId, ScanExample scanExample)
        {
            if (scanExample == null)
            {
                throw new ArgumentNullException("scanExample");
            }
            var scenarioObjectId = new ObjectId(scenarioId);

            //// Append the new example to the scenario training set
            var updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                Query<Scenario>.EQ(scenario => scenario.Id, scenarioObjectId),
                Update<Scenario>.Push(scenario => scenario.TrainingSet, scanExample),
                new MongoUpdateOptions
                {
                    WriteConcern = WriteConcern.Acknowledged
                });

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DataServiceExceptions(Properties.Resources.MsgErrorAddScanExample);
            }
        }

        /// <summary>
        /// Returns all the scenarios containing at least one of the specified features.
        /// </summary>
        /// <param name="features">The MAC addresses associated to the possible features.</param>
        /// <returns></returns>
        public IEnumerable<Scenario> GetByPossibleFeatures(IEnumerable<string> features)
        {
            if (features == null)
            {
                throw new ArgumentNullException("features");
            }
            var scenarioQuery = Query<Scenario>.In(scenario => scenario.Features.Keys, features);
            return this.MongoConnectionHandler.MongoCollection.Find(scenarioQuery);
        }

        public override void Update(Scenario entity)
        {
            throw new NotImplementedException();
        }
    }
}
