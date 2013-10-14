using System;
using System.Linq.Expressions;

namespace AttiLA.Data.Services
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using Entities;

    public abstract class EntityService<T> : IEntityService<T> where T : IMongoEntity
    {

        protected readonly MongoConnectionHandler<T> MongoConnectionHandler;

        /// <summary>
        /// Serialize the entity creating a document in the database.
        /// The document id is eventually set.
        /// </summary>
        /// <exception cref="ArgumentNullException">The parameter is null.</exception>
        /// <exception cref="DatabaseException">Write on database failure.</exception>
        /// <param name="entity"></param>
        public virtual void Create(T entity)
        {
            if(entity==null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                //// Save the entity with safe mode (WriteConcern.Acknowledged)
                var result = this.MongoConnectionHandler.MongoCollection.Save(
                    entity,
                    new MongoInsertOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });

                if (!result.Ok)
                {
                    throw new DatabaseException(Properties.Resources.MsgErrorCreate + typeof(T).ToString());
                }
            }
            catch(WriteConcernException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorCreate + typeof(T).ToString(), ex);
            }
        }

        /// <summary>
        /// Delete the document associated to the id.
        /// </summary>
        /// <param name="id">Identifier of the collection to be deleted.</param>
        public virtual void Delete(string id)
        {
            try
            {
                var result = this.MongoConnectionHandler.MongoCollection.Remove(
                    Query<T>.EQ(e => e.Id,
                    new ObjectId(id)),
                    RemoveFlags.None,
                    WriteConcern.Acknowledged);

                if (!result.Ok)
                {
                    throw new DatabaseException(Properties.Resources.MsgErrorDelete + typeof(T).ToString());
                }
            }
            catch(WriteConcernException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorDelete + typeof(T).ToString(), ex);
            }
        }

        protected EntityService()
        {
            MongoConnectionHandler = new MongoConnectionHandler<T>();
        }

        /// <summary>
        /// Deserialize into the entity the document associated to the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(string id)
        {
            var entityQuery = Query<T>.EQ(e => e.Id, new ObjectId(id));
            return this.MongoConnectionHandler.MongoCollection.FindOne(entityQuery);
        }

        public abstract void Update(T entity);
    }
}