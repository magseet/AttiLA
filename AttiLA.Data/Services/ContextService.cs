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
        public ContextService()
        {
            this.MongoConnectionHandler.MongoCollection.EnsureIndex(
                new IndexKeysBuilder().Ascending("ContextName"),
                IndexOptions.SetUnique(true));
        }

        /// <summary>
        /// Update the context details except for statistics and creation date.
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(Context entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            WriteConcernResult updateResult;
            try
            {
                updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                    Query<Context>.EQ(c => c.Id, entity.Id),
                    Update<Context>
                        .Set(c => c.ContextName, entity.ContextName)
                        .Set(c => c.NetworkInterfaces, entity.NetworkInterfaces),
                    new MongoUpdateOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });
            }
            catch (MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + entity.Id.ToString(), ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + entity.Id.ToString());
            }
        }
   

        /// <summary>
        /// Get the most recent contexts in the database.
        /// </summary>
        /// <param name="limit">The maximum number of contexts to be returned.</param>
        /// <returns></returns>
        public IEnumerable<Context> GetMostRecent(int limit)
        {
            return this.MongoConnectionHandler.MongoCollection
                .Find(new QueryDocument())
                .SetSortOrder(SortBy.Descending(Utils<Context>.MemberName(c => c.CreationDateTime)))
                .SetLimit(limit).AsEnumerable();
        }

        public void AddStatistics(string contextId, ContextStatistics statistics)
        {
            var contextObjectId = new ObjectId(contextId);

            if(statistics == null)
            {
                throw new ArgumentNullException("statistics");
            }

            WriteConcernResult updateResult;
            try
            {
                updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                    Query<Context>.EQ(c => c.Id, contextObjectId),
                    Update<Context>.Push(c => c.Statistics, statistics),
                    new MongoUpdateOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });
            }
            catch (MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + contextId, ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + contextId);
            }

        }
    }
}
