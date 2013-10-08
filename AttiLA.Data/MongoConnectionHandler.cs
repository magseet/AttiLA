using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttiLA.Data
{
    using MongoDB.Driver;
    using Entities;

    public class MongoConnectionHandler<T> where T : IMongoEntity
    {
        public MongoCollection<T> MongoCollection { get; private set; }

        public MongoConnectionHandler()
        {
            //// Get a thread-safe client object by using a connection string
            var mongoClient = new MongoClient(Properties.Settings.Default.connectionString);

            //// Get a reference to a server object from the Mongo client object
            var mongoServer = mongoClient.GetServer();

            //// Get a reference to the database object from the Mongo server object
            var db = mongoServer.GetDatabase(Properties.Settings.Default.databaseName);

            //// Get a reference to the collection object from the Mongo database object
            //// The collection name is the type converted to lowercase + "s"
            MongoCollection = db.GetCollection<T>(typeof(T).Name.ToLower() + "s");

        }

    }
}
