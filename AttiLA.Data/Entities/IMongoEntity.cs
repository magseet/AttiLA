namespace AttiLA.Data.Entities
{
    using System;
    using MongoDB.Bson;

    interface IMongoEntity
    {
        ObjectId Id { get; set; }
    }
}
