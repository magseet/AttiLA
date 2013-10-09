using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data.Services
{
    using Entities;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    public class ContextService : EntityService<Context>
    {
        /// <summary>
        /// Adds a new scan example to the training set of the context.
        /// </summary>
        /// <exception cref="DataException">Thrown when the new example is not inserted due to an error.</exception>
        /// <param name="contextId"></param>
        /// <param name="scanExample"></param>
        public void AddScanExample(string contextId, ScanExample scanExample)
        {
            if (scanExample == null)
            {
                throw new ArgumentNullException("scanExample");
            }
            var contextObjectId = new ObjectId(contextId);

            //// Append the new example to the context training set
            var updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                Query<Context>.EQ(context => context.Id, contextObjectId),
                Update<Context>.Push(context => context.TrainingSet, scanExample),
                new MongoUpdateOptions
                {
                    WriteConcern = WriteConcern.Acknowledged
                });

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DataException(Properties.Resources.MsgErrorAddScanExample);
            }
        }

        /// <summary>
        /// Returns all the contexts containing at least one of the specified features.
        /// </summary>
        /// <param name="features">The MAC addresses associated to the possible features.</param>
        /// <returns></returns>
        public IEnumerable<Context> GetByPossibleFeatures(IEnumerable<string> features)
        {
            if (features == null)
            {
                throw new ArgumentNullException("features");
            }
            var contextQuery = Query<Context>.In(context => context.Features.Keys, features);
            return this.MongoConnectionHandler.MongoCollection.Find(contextQuery);
        }


        public override void Update(Context entity)
        {
            //// TODO
            throw new NotImplementedException();
        }
    }
}
