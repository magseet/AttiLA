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
        /// Adds new scan examples to the training set.
        /// </summary>
        /// <exception cref="DataServiceExceptions">Thrown when the new example is not inserted due to an error.</exception>
        /// <param name="scenarioId"></param>
        /// <param name="scanExamples">The list of examples to add</param>
        public void AddScanExamples(string scenarioId, IEnumerable<ScanExample> scanExamples)
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
            catch(WriteConcernException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorAddScanExample, ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorAddScanExample);
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
