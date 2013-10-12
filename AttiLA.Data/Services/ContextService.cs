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
            //// TODO
            throw new NotImplementedException();
        }

        public ContextService()
        {
            this.MongoConnectionHandler.MongoCollection.EnsureIndex(
                new IndexKeysBuilder().Ascending("ContextName"),
                IndexOptions.SetUnique(true));
        }
    }
}
