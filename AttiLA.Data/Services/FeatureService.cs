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
    public class FeatureService : EntityService<Feature>
    {
        
        public IEnumerable<Feature> GetByScenario(string scenarioId)
        {
            var query = Query<Feature>.EQ(f => f.Key.ScenarioId, new ObjectId(scenarioId));
            return this.MongoConnectionHandler.MongoCollection.Find(query).AsEnumerable();
        }

        public override void Update(Feature entity)
        {
            throw new NotImplementedException();
        }        
    }
}
