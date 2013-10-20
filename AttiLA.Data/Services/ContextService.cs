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
        public override void Update(Context entity)
        {
        }

        public ContextService()
        {
            this.MongoConnectionHandler.MongoCollection.EnsureIndex(
                new IndexKeysBuilder().Ascending("ContextName"),
                IndexOptions.SetUnique(true));
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
    }
}
